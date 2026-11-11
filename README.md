## Running with Docker

Ensure the API is running locally on port `7248`. Then run:

```bash
docker build -t raceday-mvc .
docker run -d -p 8080:8080 -e "ApiSettings__BaseUrl=http://host.docker.internal:7248/" --name raceday-mvc-container raceday-mvc
