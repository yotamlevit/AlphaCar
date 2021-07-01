# AlphaCar
## _AI Car That Drives in a Simulator_

[![Build Status](https://travis-ci.org/joemccann/dillinger.svg?branch=master)](https://travis-ci.org/joemccann/dillinger)

AlphaCar project is both a Driving Simulator and a AI that drives a car.
- AI - a nural network with 4 layers, 5 inputs and 3 outputs.
- Sumilator - contains a movable car, track, borders and a folowing camera.
 🏎️🏎️🏎️

## About the ML

- Learning - Genetic Algorithem that takes 2 statistically chosen parents and merge them in a random merge for the next evolution.
- Neural Network - a nural network with 4 layers, 5 inputs and 3 outputs:
![NN Photo](https://github.com/yotamlevit/AlphaCar/blob/7dac43f4635e75cdff9fc1817bb1ed3f4bdee6ab/AlphaCarPhotos/NN_titled.PNG)
    - Inputs -  5 laser rays that measures the distances from the car to a point that the laser              hits. The laser`s diractions are: Front, front right, front lest, right and left.
![Laser top view Photo](https://github.com/yotamlevit/AlphaCar/blob/7dac43f4635e75cdff9fc1817bb1ed3f4bdee6ab/AlphaCarPhotos/LaserTopView.PNG)
    - Hidden Layers - 2 hidden layers with 4 nodes each.
    
    - Output - 3 directions of movement: Strait, tur left, turn right.
    
