# OnlineClipboard
A simple online clipboard service with support for custom ids

# Installation / Deployment

## Docker
You can deploy this Webapp via a Docker compose script:
```yaml
version: "3.9"

services:
  web:
    image: niei/onlineclipboard:latest
    container_name: onlineclipboard
    network_mode: bridge
    ports:
      - 1024:80
    restart: always
```
