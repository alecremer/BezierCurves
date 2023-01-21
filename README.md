# Unity Bezier curves
API that creates Bezier curves based on N control points.

- Edit mode debugger;


# Methods

## Curve
public Vector3 Curve(float curveStep, int controlPointIndex = 0)

Calculate Bezier curve using control points

##### Arguments
curveStep: parameter of parametric curve that walks along the curve. 0 is the start of curve, 1 is the end of curve;
controlPointIndex: index of control point in sum of all control points. For calculate whole curve, don't use this parameter.


##### Return
Point in space


## BakeCurve
public List<BakedPoint> BakeCurve(float curveStepLenght)

Calculate curve and store in list of baked point. It saves processing in game time

##### Arguments
curveStepLenght: parameter of parametric curve that walks along the curve. 0 is the start of curve, 1 is the end of curve;


##### Return
List of BakedPoint

# Classes

## BakedPoint
Precalculated curve point

##### Properties

public Vector3 point: world point
public Vector3 tangent: point tangent vector
public Vector3 normal: point normal vector


## ControlPoin
Control point of Bezier curve

##### Properties

public GameObject point: object to get transform
public Color color: color of debug point