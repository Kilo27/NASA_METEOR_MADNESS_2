from flask import Flask
from src.getTrajectory import calculate_asteroid_trajectory

app=Flask(__name__)

"""
listOfAsteroids = getAsteroidData()
they pick an id
trajectory = calculate_asteroid_trajectory(getOrbitalData(listOfAsteroids.ID),start_date, end_date, step_days=1)

return trajectory
"""
if __name__ == '__main__':
    app.run()