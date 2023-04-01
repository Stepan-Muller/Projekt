#include <float.h>
#include <math.h>

#define PI 3.1415926535
#define RATIO 53.1301023542 // uhel v rovnostrannem trojuhelniku s vyskou 1 a sirkou zakladny 1

static float degToRad(float a)
{
    return a * PI / 180.0;
}

static float capDeg(float a)
{
    if (a > 360)
        a -= 360;
    else if (a < 0)
        a += 360;

    return a;
}

static float capRad(float a)
{
    if (a > 2 * PI)
        a -= 2 * PI;
    else if (a < 0)
        a += 2 * PI;

    return a;
}

static float euclideanDistance(float aX, float aY, float bX, float bY)
{
    return sqrtf((bX - aX) * (bX - aX) + (bY - aY) * (bY - aY));
}

static float absolute(float a)
{
    if (a >= 0)
        return a;
    return -a;
}

static float sign(float a)
{
    if (a > 0)
        return 1;
    if (a < 0)
        return -1;
    return 0;
}

static int min(int a, int b)
{
    if (a < b)
        return a;
    return b;
}

static int max(int a, int b)
{
    if (a > b)
        return a;
    return b;
}