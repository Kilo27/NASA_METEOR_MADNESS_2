import requests

# Example: Astronomy Picture of the Day (APOD)
api_key = "ESoNRl5hTTqAWrQAHMoIcCcaIgJwCm5vT2LfoUih"  # replace with your own API key
start_date = "2023-10-01"
end_date = "2024-10-07"
url = f"https://api.nasa.gov/neo/rest/v1/feed?start_date=2025-10-01&end_date=2025-10-03&api_key={api_key}"

#url = f"-https://api.nasa.gov/neo/rest/v1/feed?start_date=START_DATE={start_date}&end_date={end_date}=END_DATE&api_key=API_KEY={api_key} "

response = requests.get(url)

print(response.status_code)  # should be 200 if successful
data = response.json()
print(data)