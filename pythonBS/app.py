from flask import Flask, request, jsonify
from datetime import datetime
from src.getTrajectory import calculate_asteroid_trajectory
from API_calls import getData

app=Flask(__name__)



@app.route('/trajectory')
def trajectory():
    id = request.args.get("id")
    # startdate = request.args.get("startdate")
    # end_date = request.args.get("enddate")

    return gt.calculate_asteroid_trajectory(gt.orbitalData(int(id)), "2024-10-01", "2024-10-10")
"""
listOfAsteroids = getAsteroidData()
they pick an id
trajectory = calculate_asteroid_trajectory(getOrbitalData(listOfAsteroids.ID),start_date, end_date, step_days=1)

return trajectory
"""
@app.route("/getAsteroids") #http://localhost:5000/getAsteroids?start_date=2025-10-05&weeks=1
def get_asteroids(methods="POST"):
    start_date=request.args.get('start_date')
    weeks= int(request.args.get('weeks'))
    return jsonify(getData.getAsteroids(start_date, weeks))
    
@app.route("/getOrbitalDataById")#http://localhost:5000/getOrbitalDataById?asteroid_id=2497232
def get_orbital_data(methods="POST"):
    asteroid_id=int(request.args.get("asteroid_id"))
    return jsonify(getData.orbitalData(asteroid_id))


if __name__ == '__main__':
    app.run()