FROM mcr.microsoft.com/dotnet/sdk:3.1-focal

# Install Mono and Nuget CLI
RUN apt-get -y update && \
    apt-get -y install dirmngr gnupg apt-transport-https ca-certificates && \
    apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF && \
    sh -c 'echo "deb https://download.mono-project.com/repo/ubuntu stable-focal main" > /etc/apt/sources.list.d/mono-official-stable.list' && \
    apt-get -y update && \
    apt-get -y install --no-install-recommends mono-complete nuget && \
    apt-get clean && rm -rf /var/lib/apt/lists/

RUN apt update \
    && apt-get install -y cmake \
                          ninja-build \
                          clang-11 \
                          clang-tidy-11 \
                          build-essential \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/

RUN curl https://sh.rustup.rs -sSf | sh -s -- -y
CMD [ "pwsh" ]
