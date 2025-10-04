from flask import Flask, request, jsonify
from datetime import datetime
from src import getTrajectory
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

@app.route("/calculateAsteroidTrajectory")
def get_trajectory():
    asteroid_id=int(request.args.get("asteroid_id"))
    elements = getData.orbitalData(asteroid_id)
    end_date=request.args.get("end_date")
    return jsonify(getTrajectory.calculate_asteroid_trajectory(elements, end_date))#http://localhost:5000/calculateAsteroidTrajectory?asteroid_id=2497232&end_date=2026-10-01

@app.route("/getAsteroidInfo")#http://localhost:5000/getAsteroidInfo?asteroid_id=2497232
def asteroid_info(methods="POST"):
    asteroid_id=int(request.args.get("asteroid_id"))
    return jsonify(getData.get_asteroid_info(asteroid_id))
@app.route("/getAsteroidInfoByDay")#http://localhost:5000/getAsteroidInfoByDay?asteroid_id=2497232&day=0
def asteroid_info_by_day(methods="POST"):
    asteroid_id=int(request.args.get("asteroid_id"))
    day=int(request.args.get("day"))
    return jsonify(getData.get_asteroid_info(asteroid_id, day))
if __name__ == '__main__':
    app.run()