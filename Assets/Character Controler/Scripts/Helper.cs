using UnityEngine;

public static class Helper
{
    public static float ClampAngle(float angle, float min, float max)
    {
        do
        {
            if (angle < -360) 
                angle += 360;

            if (angle > 360)
                angle -= 360;

        } while (angle < -360 || angle > 360);

        return Mathf.Clamp(angle, min, max);
    }

    public static Vector3[,] CalculatePlanePoints(Vector3 pos, int gridSize)
    {
        if (Camera.main == null)
            return null;

        var transform = Camera.main.transform;
        var halfFOV = (Camera.main.fieldOfView / 2) * Mathf.Deg2Rad;
        var aspect = Camera.main.aspect;
        var distance = Camera.main.nearClipPlane;
        var height = distance * Mathf.Tan(halfFOV) * 2;
        var width = height * aspect;

        float heightStep = height / gridSize;
        float widthStep = width / gridSize;

        Vector3[,] planePoints = new Vector3[gridSize+1, gridSize+1];

        for (int i = 0; i <= gridSize; i++)
        {
            for (int j = 0; j <= gridSize; j++)
            {
                // UpperLeft corner
                planePoints[i, j] = pos + transform.up * height / 2 - transform.right*width/2;

                planePoints[i, j] += transform.right * widthStep*j - transform.up * heightStep*i; 
            }
        }

        /*
        clipPlanePoints.LowerRight = pos + transform.right * width;
        clipPlanePoints.LowerRight -= transform.up * height;
        clipPlanePoints.LowerRight += transform.forward * distance;

        clipPlanePoints.LowerLeft = pos - transform.right * width;
        clipPlanePoints.LowerLeft -= transform.up * height;
        clipPlanePoints.LowerLeft += transform.forward * distance;

        clipPlanePoints.UpperRight = pos + transform.right * width;
        clipPlanePoints.UpperRight += transform.up * height;
        clipPlanePoints.UpperRight += transform.forward * distance;

        clipPlanePoints.UpperLeft = pos - transform.right * width;
        clipPlanePoints.UpperLeft += transform.up * height;
        clipPlanePoints.UpperLeft += transform.forward * distance;
         * */


        return planePoints;
    }

    public static Vector3 CartesianToSpherical(Vector3 cartCoords)
    {
        float radius, polar, elevation;
        if (cartCoords.x == 0)
            cartCoords.x = Mathf.Epsilon;
        radius = Mathf.Sqrt((cartCoords.x * cartCoords.x)
                        + (cartCoords.y * cartCoords.y)
                        + (cartCoords.z * cartCoords.z));
        polar = Mathf.Atan(cartCoords.z / cartCoords.x);
        if (cartCoords.x < 0)
            polar += Mathf.PI;
        elevation = Mathf.Asin(cartCoords.y / radius);

        return new Vector3(radius, polar, elevation);
    }

    public static Vector3 SphericalToCartesian(Vector3 sphericalCoords)
    {
        Vector3 cart = new Vector3();
        float a = sphericalCoords.x * Mathf.Cos(sphericalCoords.z);
        cart.x = a * Mathf.Cos(sphericalCoords.y);
        cart.y = sphericalCoords.x * Mathf.Sin(sphericalCoords.z);
        cart.z = a * Mathf.Sin(sphericalCoords.y);
        return cart;
    }
}
