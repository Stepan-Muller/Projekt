#include <float.h>
#include <math.h>

#define PI 3.1415926535
#define RATIO 53.1301023542 // uhel v rovnostrannem trojuhelniku s vyskou 1 a sirkou zakladny 1

/* Ruzne matematicke funkce */

/* Prevede stupne na radiany */
static float degToRad(float a)
{
    return a * PI / 180.0;
}

/* Zastavi stupne na 360 a 0 */
static float capDeg(float a)
{
    if (a > 360)
        a -= 360;
    else if (a < 0)
        a += 360;

    return a;
}

/* Zastavi radiany na 2PI a 0 */
static float capRad(float a)
{
    if (a > 2 * PI)
        a -= 2 * PI;
    else if (a < 0)
        a += 2 * PI;

    return a;
}

/* Vzdalenost dvou bodu na 2D plose */
static float euclideanDistance(float aX, float aY, float bX, float bY)
{
    return sqrtf((bX - aX) * (bX - aX) + (bY - aY) * (bY - aY));
}

/* Absoulutni hodnota cisla */
static float absolute(float a)
{
    if (a >= 0)
        return a;
    return -a;
}

/* Znamenko cisla - bud 1, 0, nebo -1 */
static float sign(float a)
{
    if (a > 0)
        return 1;
    if (a < 0)
        return -1;
    return 0;
}

/* Vybre mensi z cisel */
static int min(int a, int b)
{
    if (a < b)
        return a;
    return b;
}

/* Vybere vetsi z cisel */
static int max(int a, int b)
{
    if (a > b)
        return a;
    return b;
}