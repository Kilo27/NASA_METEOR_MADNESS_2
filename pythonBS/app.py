from flask import Flask, request, jsonify
from datetime import datetime
from src.getTrajectory import calculate_asteroid_trajectory
from API_calls import getData

app=Flask(__name__)

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
    print(getTrajectory.calculate_asteroid_trajectory(elements, end_date))
    return jsonify(getTrajectory.calculate_asteroid_trajectory(elements, end_date))#http://localhost:5000/calculateAsteroidTrajectory?asteroid_id=2497232&end_date=2026-10-01
    
@app.route("/getOrbitalDataById")#http://localhost:5000/getOrbitalDataById?asteroid_id=2497232
def get_orbital_data(methods="POST"):
    asteroid_id=int(request.args.get("asteroid_id"))
    return jsonify(getData.orbitalData(asteroid_id))


if __name__ == '__main__':
    app.run()