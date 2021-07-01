# AlphaCar
## _AI Car That Drives in a Simulator:_



AlphaCar project is both a Driving Simulator and a AI that drives a car.
- AI - a nural network with 4 layers, 5 inputs and 3 outputs.
- Sumilator - contains a movable car, track, borders and a folowing camera.
 🏎️🏎️🏎️

## About the ML

- Learning - Genetic Algorithem that takes 2 statistically chosen parents and merge them in a random merge for the next evolution.
- Neural Network - a nural network with 4 layers, 5 inputs and 3 outputs:
<img src="https://github.com/yotamlevit/AlphaCar/blob/7dac43f4635e75cdff9fc1817bb1ed3f4bdee6ab/AlphaCarPhotos/NN_titled.PNG" width="60%" class="center">
    - Inputs -  5 laser rays that measures the distances from the car
                to a point that the laser hits.
                The laser`s diractions are: Front, front right, front lest, right and left.
    
<img src="https://github.com/yotamlevit/AlphaCar/blob/7dac43f4635e75cdff9fc1817bb1ed3f4bdee6ab/AlphaCarPhotos/LaserTopView.PNG" width="40%" class="center">
    - Hidden Layers - 2 hidden layers with 4 nodes each.
    
    - Output - 3 directions of movement: Strait, tur left, turn right.
    
