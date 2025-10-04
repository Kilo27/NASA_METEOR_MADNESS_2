from flask import Flask, request
import requests

import src.getTrajectory as gt
from python.calculateOrbitalData import end_date

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
if __name__ == '__main__':
    app.run()