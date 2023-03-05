#include <GLFW/glfw3.h>
#include <math.h>
#include <float.h>

#define PI 3.1415926535
#define DEG_2_RAD 0.0174533 // jeden stupen v rad
#define RATIO 53.1301023514 // uhel v rovnostrannem trojuhelniku s vyskou 1 a sirkou zakladny 1, pouzito pro vypocet vysky car

#define RENDER_DISTANCE 8

#define RESOLUTION 1.0 // rozliseni - sirka sloupcu
#define FOW 60.0
#define TURN_SPEED 3
#define MOVE_SPEED 200

bool debug = false;

float playerX, playerY, playerDeltaX, playerDeltaY, playerAngle; // Player pos

int screenWidth, screenHeight;

void key_callback(GLFWwindow* window, int key, int scancode, int action, int mods)
{
    if (key == GLFW_KEY_F3 && action == GLFW_PRESS)
        debug = !debug;
}

void window_size_callback(GLFWwindow* window, int width, int height)
{
    screenWidth = width;
    screenHeight = height;

    glLoadIdentity();
    glOrtho(0, width, height, 0, -1, 1);
    glViewport(0, 0, width, height);
}

void drawPlayer()
{
    glColor3f(1, 1, 0);
    glPointSize(8);
    glBegin(GL_POINTS);
    glVertex2i(playerX, playerY);
    glEnd();

    glLineWidth(3);
    glBegin(GL_LINES);
    glVertex2i(playerX, playerY);
    glVertex2i(playerX + playerDeltaX * 20, playerY + playerDeltaY * 20);
    glEnd();
}

int mapX = RENDER_DISTANCE, mapY = RENDER_DISTANCE, mapScale = 64;
int map[] =
{
    1, 1, 1, 1, 1, 1, 1, 1,
    1, 0, 0, 0, 0, 0, 0, 1,
    1, 0, 1, 1, 0, 1, 1, 1,
    1, 0, 1, 0, 0, 0, 0, 1,
    1, 0, 1, 0, 0, 0, 0, 1,
    1, 0, 1, 0, 0, 1, 0, 1,
    1, 0, 1, 0, 0, 0, 0, 1,
    1, 1, 1, 1, 1, 1, 1, 1,
};

void drawMap()
{
    int x, y, xOffset, yOffset;
    for (y = 0; y < mapY; y++)
    {
        for (x = 0; x < mapX; x++)
        {
            if (map[y * mapX + x] == 1)
                glColor3f(1, 1, 1);
            else
                glColor3f(0, 0, 0);

            xOffset = x * mapScale;
            yOffset = y * mapScale;
            
            glBegin(GL_QUADS);
            glVertex2i(xOffset        + 1, yOffset        + 1);
            glVertex2i(xOffset        + 1, yOffset + mapScale - 1);
            glVertex2i(xOffset + mapScale - 1, yOffset + mapScale - 1);
            glVertex2i(xOffset + mapScale - 1, yOffset        + 1);
            glEnd();
        }
    }
}

float dist(float aX, float aY, float bX, float bY)
{
    return sqrt((bX- aX) * (bX - aX) + (bY - aY) * (bY - aY));
}

void draw3D()
{
    int ray, mapPointerX, mapPointerY, mapPointer, deltaOffset;
    float rayX, rayY, rayAngle, xOffset, yOffset, distance;
    rayAngle = playerAngle - DEG_2_RAD * FOW / 2;
    if (rayAngle < 0)
        rayAngle += 2 * PI;
    else if (rayAngle > 2 * PI)
        rayAngle -= 2 * PI;
    
    for (ray = 0; ray <= screenWidth / RESOLUTION; ray++)
    {
        // Horizontální
        deltaOffset = 0;
        float distanceHorizontal = FLT_MAX, horizontalX = playerX, horizontalY = playerY;
        float arcTan = -1 / tan(rayAngle);

        if (rayAngle == 0 || rayAngle == PI)
        {
            rayX = playerX;
            rayY = playerY;
            deltaOffset = 8;
        }
        else
        {
            // Nahoru
            if (rayAngle > PI)
            {
                rayY = (((int)playerY >> 6) << 6) - 0.001;
                rayX = (playerY - rayY) * arcTan + playerX;
                yOffset = -64;
                xOffset = -yOffset * arcTan;
            }

            // Dolu
            else
            {
                rayY = (((int)playerY >> 6) << 6) + 64;
                rayX = (playerY - rayY) * arcTan + playerX;
                yOffset = 64;
                xOffset = -yOffset * arcTan;
            }

            while (deltaOffset < RENDER_DISTANCE)
            {
                mapPointerX = (int)(rayX) >> 6;
                mapPointerY = (int)(rayY) >> 6;
                mapPointer = mapPointerY * mapX + mapPointerX;

                if (mapPointer > 0 && mapPointer < mapX * mapY && map[mapPointer] == 1)
                {
                    horizontalX = rayX;
                    horizontalY = rayY;
                    distanceHorizontal = dist(playerX, playerY, horizontalX, horizontalY);
                    deltaOffset = 8;
                }
                else
                {
                    rayX += xOffset;
                    rayY += yOffset;
                    deltaOffset++;
                }
            }
        }

        // Vertikální
        deltaOffset = 0;
        float distanceVertical = FLT_MAX, verticalX = playerX, verticalY = playerY;
        float negativeTan = -tan(rayAngle);

        if (rayAngle == 0 || rayAngle == PI)
        {
            rayX = playerX;
            rayY = playerY;
        }
        else
        {
            // Doleva
            if (rayAngle > PI / 2 && rayAngle < 3 * PI / 2)
            {
                rayX = (((int)playerX >> 6) << 6) - 0.001;
                rayY = (playerX - rayX) * negativeTan + playerY;
                xOffset = -64;
                yOffset = -xOffset * negativeTan;
            }

            // Doprava
            else
            {
                rayX = (((int)playerX >> 6) << 6) + 64;
                rayY = (playerX - rayX) * negativeTan + playerY;
                xOffset = 64;
                yOffset = -xOffset * negativeTan;
            }

            while (deltaOffset < RENDER_DISTANCE)
            {
                mapPointerX = (int)(rayX) >> 6;
                mapPointerY = (int)(rayY) >> 6;
                mapPointer = mapPointerY * mapX + mapPointerX;

                if (mapPointer > 0 && mapPointer < mapX * mapY && map[mapPointer] == 1)
                {
                    verticalX = rayX;
                    verticalY = rayY;
                    distanceVertical = dist(playerX, playerY, verticalX, verticalY);
                    break;
                }
                else
                {
                    rayX += xOffset;
                    rayY += yOffset;
                    deltaOffset++;
                }
            }
        }

        if (distanceVertical < distanceHorizontal)
        {
            rayX = verticalX;
            rayY = verticalY;
            distance = distanceVertical;
            glColor3f(0.9, 0, 0);
        }
        else
        {
            rayX = horizontalX;
            rayY = horizontalY;
            distance = distanceHorizontal;
            glColor3f(0.7, 0, 0);
        }

        // Kreslit 3D
        float changeAngle = playerAngle - rayAngle;
        if (changeAngle < 0)
            changeAngle += 2 * PI;
        else if (changeAngle > 2 * PI)
            changeAngle -= 2 * PI;

        // Oprava fisheye
        distance = distance * cos(changeAngle);
        
        float lineHeight = (mapScale * screenWidth) * (RATIO / FOW) / distance;
        if (lineHeight > screenHeight)
            lineHeight = screenHeight;

        float lineOffset = (screenHeight - lineHeight) / 2;

        glBegin(GL_QUADS);
        glVertex2i(ray * RESOLUTION, lineOffset);
        glVertex2i(ray * RESOLUTION, lineHeight + lineOffset);
        glVertex2i((ray + 1) * RESOLUTION, lineHeight + lineOffset);
        glVertex2i((ray + 1) * RESOLUTION, lineOffset);
        glEnd();

        rayAngle += FOW / (screenWidth / RESOLUTION) * DEG_2_RAD;
        if (rayAngle < 0)
            rayAngle += 2 * PI;
        else if (rayAngle > 2 * PI)
            rayAngle -= 2 * PI;
    }
}

int main(void)
{
    GLFWwindow* window;

    /* Initialize the library */
    if (!glfwInit())
        return -1;

    /* Create a windowed mode window and its OpenGL context */
    window = glfwCreateWindow(1280, 720, "Hello World", NULL, NULL);
    if (!window)
    {
        glfwTerminate();
        return -1;
    }

    /* Make the window's context current */
    glfwMakeContextCurrent(window);

    glfwSwapInterval(1);

    glfwSetKeyCallback(window, key_callback);
    glfwSetWindowSizeCallback(window, window_size_callback);

    glClearColor(0.3, 0.3, 0.3, 0);
    glOrtho(0, 1280, 720, 0, -1, 1);
    screenWidth = 1280;
    screenHeight = 720;
    
    playerX = 300;
    playerY = 300;
    playerDeltaX = cos(playerAngle);
    playerDeltaY = sin(playerAngle);

    float lastTime = glfwGetTime();

    /* Loop until the user closes the window */
    while (!glfwWindowShouldClose(window))
    {
        float deltaTime = glfwGetTime() - lastTime;
        lastTime = glfwGetTime();
        
        // Inputy
        if (glfwGetKey(window, GLFW_KEY_A) == GLFW_PRESS)
        {
            playerAngle -= TURN_SPEED * deltaTime;
            if (playerAngle < 0) 
                playerAngle += 2 * PI;
            playerDeltaX = cos(playerAngle);
            playerDeltaY = sin(playerAngle);
        }

        if (glfwGetKey(window, GLFW_KEY_D) == GLFW_PRESS)
        {
            playerAngle += TURN_SPEED * deltaTime;
            if (playerAngle > 2 * PI) 
                playerAngle -= 2 * PI;
            playerDeltaX = cos(playerAngle);
            playerDeltaY = sin(playerAngle);
        }

        int xo = 0;
        if (playerDeltaX < 0)
            xo = -20;
        else
            xo = 20;
        int yo = 0;
        if (playerDeltaY < 0)
            yo = -20;
        else
            yo = 20;
        int ipx = playerX / 64.0, ipx_add_xo = (playerX + xo) / 64.0, ipx_sub_xo = (playerX - xo) / 64.0;
        int ipy = playerY / 64.0, ipy_add_yo = (playerY + yo) / 64.0, ipy_sub_yo = (playerY - yo) / 64.0;

        if (glfwGetKey(window, GLFW_KEY_W) == GLFW_PRESS)
        {
            if (map[ipy * mapX + ipx_add_xo] == 0)
                playerX += playerDeltaX * deltaTime * MOVE_SPEED;
            if (map[ipy_add_yo * mapX + ipx] == 0)
                playerY += playerDeltaY * deltaTime * MOVE_SPEED;
        }

        if (glfwGetKey(window, GLFW_KEY_S) == GLFW_PRESS)
        {
            if (map[ipy * mapX + ipx_sub_xo] == 0)
                playerX -= playerDeltaX * deltaTime * MOVE_SPEED;
            if (map[ipy_sub_yo * mapX + ipx] == 0)
                playerY -= playerDeltaY * deltaTime * MOVE_SPEED;
        }

        // Renderovani
        glClear(GL_COLOR_BUFFER_BIT);

        draw3D();
        if (debug)
        {
            drawMap();
            drawPlayer();
        }

        glfwSwapBuffers(window);

        /* Poll for and process events */
        glfwPollEvents();
    }

    glfwTerminate();
    return 0;
}