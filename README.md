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
    volumes:
      - stored_data:/app/db
    ports:
      - 1003:80
    restart: always

volumes:
  stored_data:
```
