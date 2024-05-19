# Use an official Node.js base image
FROM node:18

# Set the working directory
WORKDIR /github/workspace

# Install dependencies required by Puppeteer
RUN apt-get update && apt-get install -y wget gnupg ca-certificates procps libnss3 libxss1 fonts-liberation libappindicator3-1 xdg-utils

# Additional dependencies for running headless Chrome
RUN apt-get install -y libgbm1 libasound2

# Install Pandoc and LaTeX for PDF generation
RUN apt-get install -y pandoc texlive-latex-base texlive-fonts-recommended texlive-extra-utils texlive-latex-extra

# Install npm global packages
RUN npm install -g @mermaid-js/mermaid-cli

# Copy your repository files into the Docker image
COPY . .

# Command to run on container start
CMD ["bash", "./extract_and_to_pdf.sh", "RequirementsAnalysis.md"]