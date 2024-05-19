#!/bin/bash

# Check if an input filename is provided
if [ -z "$1" ]; then
    echo "No input Markdown file specified."
    exit 1
fi

input_file="$1"
output_dir="./mermaid_diagrams"
pdf_output_dir="./pdfs"

# Create necessary directories
mkdir -p $output_dir
mkdir -p $pdf_output_dir

# Extract Mermaid blocks and render them using mmdc
grep -Pzo '`{3}mermaid[\s\S]*?`{3}' "$input_file" | 
awk 'BEGIN{RS="```"; i=0} /mermaid/ {sub(/mermaid/, ""); print > "'$output_dir'/diagram" ++i ".mmd"}'

# Render .mmd files to .png
for mmd_file in $output_dir/*.mmd; do
    mmdc -i "$mmd_file" -o "${mmd_file%.mmd}.png"
done

# Replace Mermaid code blocks in the Markdown file with references to the PNG i1mages
awk -v outdir="$output_dir" '
    BEGIN {RS="```"; ORS=""}
    /mermaid/ {getline; print "![](" outdir "/diagram" ++diagram_count ".png)\n"}
    !/mermaid/ {print $0 (RT ? "" : "")}
' "$input_file" > "${input_file%.md}_modified.md"

# Convert the modified Markdown to PDF
pandoc "${input_file%.md}_modified.md" -o "$pdf_output_dir/${input_file%.md}.pdf"

echo "PDF with Mermaid diagrams has been generated in $pdf_output_dir"


# Cleanup

rm -r $output_dir
rm "${input_file%.md}_modified.md"