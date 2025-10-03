from flask import Flask

app=Flask(__name__)

@app.route("/getMeteorData")
def hello_world():
	return "meteordata"

if __name__ == '__main__':
    app.run()