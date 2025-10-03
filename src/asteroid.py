import requests

API_KEY = 'XAVKddnzOUKh52sgb0hZNm2PO8CPThNVfsEj2ieV'

start_date = "2020-09-01"
end_date = "2020-09-30"

x = requests.get(f"https://api.nasa.gov/neo/rest/v1/feed?start_date={start_date}&end_date={end_date}&api_key={API_KEY}")

print(x.text)