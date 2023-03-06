#include <GLFW/glfw3.h>
#include <stdbool.h>

#include <stdio.h>

#include "util.c"

#include "textures/textureMap.ppm"
#include "textures/skyBox.ppm"

#define RATIO 53.1301023514 // uhel v rovnostrannem trojuhelniku s vyskou 1 a sirkou zakladny 1, pouzito pro vypocet vysky car

#define RENDER_DISTANCE 8
#define PLAYER_SIZE 0.3 // velikost hrace v pixelech
#define TEXTURE_RESOLUTION 16 // sirka a vyska textur v pixelech

#define DEFAULT_WIDTH 1920
#define DEFAULT_HEIGHT 1080

#define FOV 60.0
#define TURN_SPEED 0.002
#define MOVE_SPEED 3

bool debug = false;
bool menu = false;
bool resetMouse = false;

float playerX, playerY, playerDeltaX, playerDeltaY, playerAngle;

int screenWidth, screenHeight;

void key_callback(GLFWwindow* window, int key, int scancode, int action, int mods)
{
    if (key == GLFW_KEY_F3 && action == GLFW_PRESS)
        debug = !debug;
    if (key == GLFW_KEY_ESCAPE && action == GLFW_PRESS)
    {
        menu = !menu;
        if (menu)
            glfwSetInputMode(window, GLFW_CURSOR, GLFW_CURSOR_NORMAL);
        else
            glfwSetInputMode(window, GLFW_CURSOR, GLFW_CURSOR_DISABLED);
        resetMouse = true;
    }
}

float lastXpos, lastYpos;

void mouse_callback(GLFWwindow* window, double xpos, double ypos)
{
    // pokud je v menu, nehybat s kamerou
    if (menu)
        return;
    
    if (resetMouse)
    {
        lastXpos = xpos;
        resetMouse = false;
    }

    float xOffset = xpos - lastXpos;
    lastXpos = xpos;

    playerAngle = capRad(playerAngle + xOffset * TURN_SPEED);

    playerDeltaX = cos(playerAngle);
    playerDeltaY = sin(playerAngle);
}

void window_size_callback(GLFWwindow* window, int width, int height)
{
    screenWidth = width;
    screenHeight = height;

    glLoadIdentity();
    glOrtho(0, width, height, 0, -1, 1);
    glViewport(0, 0, width, height);
}

int mapX = RENDER_DISTANCE, mapY = RENDER_DISTANCE;
int mapWalls[] =
{
    1, 1, 1, 1, 1, 1, 1, 1,
    1, 0, 0, 0, 0, 0, 0, 1,
    1, 0, 1, 1, 0, 1, 1, 1,
    1, 0, 1, 0, 0, 0, 0, 1,
    1, 0, 1, 0, 0, 0, 0, 1,
    1, 0, 1, 0, 0, 2, 0, 1,
    1, 0, 1, 0, 0, 0, 0, 1,
    1, 1, 1, 1, 1, 1, 1, 1,
};
int mapFloors[] = 
{
    2, 2, 2, 2, 2, 2, 2, 2,
    2, 1, 1, 1, 1, 1, 1, 2,
    2, 1, 2, 2, 1, 2, 2, 2,
    2, 1, 2, 2, 2, 2, 2, 2,
    2, 1, 2, 2, 2, 2, 2, 2,
    2, 1, 2, 2, 2, 2, 2, 2,
    2, 1, 2, 2, 2, 2, 0, 2,
    2, 2, 2, 2, 2, 2, 2, 2,
};
int mapCeilings[] = 
{
    2, 1, 2, 1, 2, 1, 2, 1,
    1, 2, 1, 2, 1, 2, 1, 2,
    2, 1, 2, 1, 0, 1, 2, 1,
    1, 2, 1, 0, 0, 0, 0, 2,
    2, 1, 2, 0, 0, 0, 0, 1,
    1, 2, 1, 0, 0, 2, 1, 2,
    2, 1, 2, 0, 0, 1, 2, 1,
    1, 2, 1, 2, 1, 2, 1, 2,
};

void movePlayer(GLFWwindow* window, float deltaTime)
{
    // pokud je v menu, nehybat s hracem
    if (menu)
        return;
    
    if (glfwGetKey(window, GLFW_KEY_W) == GLFW_PRESS)
    {
        if (mapWalls[(int)((playerY)) * mapX + (int)((playerX + PLAYER_SIZE * sign(playerDeltaX)))] == 0)
            playerX += playerDeltaX * deltaTime * MOVE_SPEED;
        if (mapWalls[(int)((playerY + PLAYER_SIZE * sign(playerDeltaY))) * mapX + (int)((playerX))] == 0)
            playerY += playerDeltaY * deltaTime * MOVE_SPEED;
    }

    if (glfwGetKey(window, GLFW_KEY_S) == GLFW_PRESS)
    {
        if (mapWalls[(int)((playerY)) * mapX + (int)((playerX - PLAYER_SIZE * sign(playerDeltaX)))] == 0)
            playerX -= playerDeltaX * deltaTime * MOVE_SPEED;
        if (mapWalls[(int)((playerY - PLAYER_SIZE * sign(playerDeltaY))) * mapX + (int)((playerX))] == 0)
            playerY -= playerDeltaY * deltaTime * MOVE_SPEED;
    }

    if (glfwGetKey(window, GLFW_KEY_A) == GLFW_PRESS)
    {
        if (mapWalls[(int)((playerY)) * mapX + (int)((playerX + PLAYER_SIZE * sign(playerDeltaY)))] == 0)
            playerX += playerDeltaY * deltaTime * MOVE_SPEED;
        if (mapWalls[(int)((playerY - PLAYER_SIZE * sign(playerDeltaX))) * mapX + (int)((playerX))] == 0)
            playerY -= playerDeltaX * deltaTime * MOVE_SPEED;
    }

    if (glfwGetKey(window, GLFW_KEY_D) == GLFW_PRESS)
    {
        if (mapWalls[(int)((playerY)) * mapX + (int)((playerX - PLAYER_SIZE * sign(playerDeltaY)))] == 0)
            playerX -= playerDeltaY * deltaTime * MOVE_SPEED;
        if (mapWalls[(int)((playerY + PLAYER_SIZE * sign(playerDeltaX))) * mapX + (int)((playerX))] == 0)
            playerY += playerDeltaX * deltaTime * MOVE_SPEED;
    }
}

void drawDebug()
{
    // nakreslit mapu
    glPointSize(1);
    glBegin(GL_POINTS);

    for (int y = 0; y < TEXTURE_RESOLUTION * mapY; y++)
    {
        for (int x = 0; x < TEXTURE_RESOLUTION * mapX; x++)
        {
            int texture = mapFloors[(int)(y / TEXTURE_RESOLUTION) * mapX + (int)(x / TEXTURE_RESOLUTION)];
            
            if (texture == 0)
                glColor3f(0, 0, 0);
            else
            {
                float textureX = (int)(x) % TEXTURE_RESOLUTION;
                float textureY = (int)(y) % TEXTURE_RESOLUTION;

                int red = textureMap[((texture - 1) * TEXTURE_RESOLUTION * TEXTURE_RESOLUTION + (int)textureY * TEXTURE_RESOLUTION + (int)textureX) * 3];
                int green = textureMap[((texture - 1) * TEXTURE_RESOLUTION * TEXTURE_RESOLUTION + (int)textureY * TEXTURE_RESOLUTION + (int)textureX) * 3 + 1];
                int blue = textureMap[((texture - 1) * TEXTURE_RESOLUTION * TEXTURE_RESOLUTION + (int)textureY * TEXTURE_RESOLUTION + (int)textureX) * 3 + 2];

                glColor3ub(red, green, blue);
            }

            glVertex2i(x, y);
        }
    }

    for (int y = 0; y < TEXTURE_RESOLUTION * mapY; y++)
    {
        for (int x = 0; x < TEXTURE_RESOLUTION * mapX; x++)
        {
            int texture = mapWalls[(int)(y / TEXTURE_RESOLUTION) * mapX + (int)(x / TEXTURE_RESOLUTION)];

            if (texture == 0)
                glColor3f(0, 0, 0);
            else
            {
                float textureX = (int)(x) % TEXTURE_RESOLUTION;
                float textureY = (int)(y) % TEXTURE_RESOLUTION;

                int red = textureMap[((texture - 1) * TEXTURE_RESOLUTION * TEXTURE_RESOLUTION + (int)textureY * TEXTURE_RESOLUTION + (int)textureX) * 3];
                int green = textureMap[((texture - 1) * TEXTURE_RESOLUTION * TEXTURE_RESOLUTION + (int)textureY * TEXTURE_RESOLUTION + (int)textureX) * 3 + 1];
                int blue = textureMap[((texture - 1) * TEXTURE_RESOLUTION * TEXTURE_RESOLUTION + (int)textureY * TEXTURE_RESOLUTION + (int)textureX) * 3 + 2];

                glColor3ub(red, green, blue);
            }

            glVertex2i(x + TEXTURE_RESOLUTION * (mapX + 1), y);
        }
    }

    for (int y = 0; y < TEXTURE_RESOLUTION * mapY; y++)
    {
        for (int x = 0; x < TEXTURE_RESOLUTION * mapX; x++)
        {
            int texture = mapCeilings[(int)(y / TEXTURE_RESOLUTION) * mapX + (int)(x / TEXTURE_RESOLUTION)];

            if (texture == 0)
                glColor3f(0, 0, 0);
            else
            {
                float textureX = (int)(x) % TEXTURE_RESOLUTION;
                float textureY = (int)(y) % TEXTURE_RESOLUTION;

                int red = textureMap[((texture - 1) * TEXTURE_RESOLUTION * TEXTURE_RESOLUTION + (int)textureY * TEXTURE_RESOLUTION + (int)textureX) * 3];
                int green = textureMap[((texture - 1) * TEXTURE_RESOLUTION * TEXTURE_RESOLUTION + (int)textureY * TEXTURE_RESOLUTION + (int)textureX) * 3 + 1];
                int blue = textureMap[((texture - 1) * TEXTURE_RESOLUTION * TEXTURE_RESOLUTION + (int)textureY * TEXTURE_RESOLUTION + (int)textureX) * 3 + 2];

                glColor3ub(red, green, blue);
            }

            glVertex2i(x + 2 * TEXTURE_RESOLUTION * (mapX + 1), y);
        }
    }

    glEnd();

    // nakreslit na mapu hrace
    glColor3f(1, 1, 0);
    glPointSize(4);
    glBegin(GL_POINTS);
    glVertex2i( playerX                   * TEXTURE_RESOLUTION, playerY * TEXTURE_RESOLUTION);
    glVertex2i((playerX +      mapX + 1)  * TEXTURE_RESOLUTION, playerY * TEXTURE_RESOLUTION);
    glVertex2i((playerX + 2 * (mapX + 1)) * TEXTURE_RESOLUTION, playerY * TEXTURE_RESOLUTION);
    glEnd();

    // ukazat jakym smerem hrac kouka
    glLineWidth(2);
    glBegin(GL_LINES);
    glVertex2i( playerX                   * TEXTURE_RESOLUTION, playerY * TEXTURE_RESOLUTION);
    glVertex2i( playerX                   * TEXTURE_RESOLUTION + playerDeltaX * 10, playerY * TEXTURE_RESOLUTION + playerDeltaY * 10);
    glVertex2i((playerX +      mapX + 1)  * TEXTURE_RESOLUTION, playerY * TEXTURE_RESOLUTION);
    glVertex2i((playerX +      mapX + 1)  * TEXTURE_RESOLUTION + playerDeltaX * 10, playerY * TEXTURE_RESOLUTION + playerDeltaY * 10);
    glVertex2i((playerX + 2 * (mapX + 1)) * TEXTURE_RESOLUTION, playerY * TEXTURE_RESOLUTION);
    glVertex2i((playerX + 2 * (mapX + 1)) * TEXTURE_RESOLUTION + playerDeltaX * 10, playerY * TEXTURE_RESOLUTION + playerDeltaY * 10);
    glEnd();
}

void draw3D()
{
    float rayX, rayY, xOffset, yOffset;
    float rayAngle = capRad(playerAngle - degToRad(FOV / 2));
    
    glPointSize(1);
    glBegin(GL_POINTS);

    // pro kazdy paprsek
    for (int ray = 0; ray <= screenWidth; ray++)
    {
        // horizontalni kolize
        float distanceHorizontal = FLT_MAX, horizontalX = playerX, horizontalY = playerY;
        int horizontalTexture;
        float arcTan = -1 / tan(rayAngle);

        //  pokud je paprsek vertikalni => nemuze mit horizontalni kolize
        if (rayAngle == 0 || rayAngle == PI)
        {
            rayX = playerX;
            rayY = playerY;
        }
        else
        {
            // pokud paprsek miri nahoru
            if (rayAngle > PI)
            {
                rayY = (int)playerY - 0.000001;
                rayX = (playerY - rayY) * arcTan + playerX;
                yOffset = -1;
                xOffset = -yOffset * arcTan;
            }

            // pokud paprsek miri dolu
            else
            {
                rayY = (int)playerY + 1;
                rayX = (playerY - rayY) * arcTan + playerX;
                yOffset = 1;
                xOffset = -yOffset * arcTan;
            }

            // hledat kolizi
            for (int i = 0; i < RENDER_DISTANCE; i++)
            {
                int mapPointer = (int)(rayY)* mapX + (int)(rayX);

                // pokud trefil
                if (mapPointer > 0 && mapPointer < mapX * mapY && mapWalls[mapPointer] > 0)
                {
                    // ulozit trefeneou pozici, vzdalenost
                    horizontalX = rayX;
                    horizontalY = rayY;

                    distanceHorizontal = euclideanDistance(playerX, playerY, horizontalX, horizontalY);
                    horizontalTexture = mapWalls[mapPointer];
                    break;
                }
                
                // pokud netrefil, pokracovat
                rayX += xOffset;
                rayY += yOffset;
            }
        }

        // vertikalni kolize
        float distanceVertical = FLT_MAX, verticalX = playerX, verticalY = playerY;
        int verticalTexture;
        float negativeTan = -tan(rayAngle);

        //  pokud je paprsek horizontalni => nemuze mit vertikalni kolize
        if (rayAngle == PI / 2 || rayAngle == 3 * PI / 2)
        {
            rayX = playerX;
            rayY = playerY;
        }
        else
        {
            // pokud paprsek miri doleva
            if (rayAngle > PI / 2 && rayAngle < 3 * PI / 2)
            {
                rayX = (int)playerX - 0.000001;
                rayY = (playerX - rayX) * negativeTan + playerY;
                xOffset = -1;
                yOffset = -xOffset * negativeTan;
            }

            // pokud paprsek miri doprava
            else
            {
                rayX = (int)playerX + 1;
                rayY = (playerX - rayX) * negativeTan + playerY;
                xOffset = 1;
                yOffset = -xOffset * negativeTan;
            }

            // hledat kolizi
            for (int i = 0; i < RENDER_DISTANCE; i++)
            {
                int mapPointer = (int)(rayY)* mapX + (int)(rayX);

                // pokud trefil
                if (mapPointer > 0 && mapPointer < mapX * mapY && mapWalls[mapPointer] > 0)
                {
                    // ulozit trefeneou pozici, vzdalenost
                    verticalX = rayX;
                    verticalY = rayY;

                    distanceVertical = euclideanDistance(playerX, playerY, verticalX, verticalY);
                    verticalTexture = mapWalls[mapPointer];
                    break;
                }
                
                // pokud netrefil, pokracovat
                rayX += xOffset;
                rayY += yOffset;
            }
        }

        float distance, shade, textureX;
        int texture;

        // ponechat si blizsi trefu (horizontalni, nebo vertikalni)
        if (distanceVertical < distanceHorizontal)
        {
            rayX = verticalX;
            rayY = verticalY;
            distance = distanceVertical;
            shade = 0.5;
            texture = verticalTexture;

            textureX = (int)(rayY * TEXTURE_RESOLUTION) % TEXTURE_RESOLUTION;
            if (rayAngle > PI / 2 && rayAngle < 3 * PI  / 2)
                textureX = TEXTURE_RESOLUTION - 1 - textureX;
        }
        else
        {
            rayX = horizontalX;
            rayY = horizontalY;
            distance = distanceHorizontal;
            shade = 1;
            texture = horizontalTexture;

            textureX = (int)(rayX * TEXTURE_RESOLUTION) % TEXTURE_RESOLUTION;
            if (rayAngle < PI)
                textureX = TEXTURE_RESOLUTION - 1 - textureX;
        }

        // nakreslit zdi
        float changeAngle = playerAngle - rayAngle;
        if (changeAngle < 0)
            changeAngle += 2 * PI;
        else if (changeAngle > 2 * PI)
            changeAngle -= 2 * PI;
        
        distance = distance * cos(changeAngle); // oprava fisheye
        
        float lineHeight = screenWidth * (RATIO / FOV) / distance;

        float textureYStep = TEXTURE_RESOLUTION / (float)lineHeight;
        float textureYOffset = 0;

        if (lineHeight > screenHeight)
        {
            textureYOffset = (lineHeight - (float)screenHeight) / 2.0;
            lineHeight = screenHeight;
        }

        float lineOffset = (screenHeight - lineHeight) / 2;

        float textureY = textureYOffset * textureYStep;

        for (int y = 0; y < lineHeight; y++)
        {
            int red = textureMap[((texture - 1) * TEXTURE_RESOLUTION * TEXTURE_RESOLUTION + (int)textureY * TEXTURE_RESOLUTION + (int)textureX) * 3] * shade;
            int green = textureMap[((texture - 1) * TEXTURE_RESOLUTION * TEXTURE_RESOLUTION + (int)textureY * TEXTURE_RESOLUTION + (int)textureX) * 3 + 1] * shade;
            int blue = textureMap[((texture - 1) * TEXTURE_RESOLUTION * TEXTURE_RESOLUTION + (int)textureY * TEXTURE_RESOLUTION + (int)textureX) * 3 + 2] * shade;

            glColor3ub(red, green, blue);
            glVertex2i(ray, y + lineOffset);

            textureY += textureYStep;
        }

        // nakreslit podlahu
        for (int y = lineOffset + lineHeight; y < screenHeight; y++)
        {
            float dy = y - (screenHeight / 2.0);

            float flatX = playerX + cos(rayAngle) * screenWidth * RATIO / FOV / 2 / dy / cos(changeAngle);
            float flatY = playerY + sin(rayAngle) * screenWidth * RATIO / FOV / 2 / dy / cos(changeAngle);

            float textureX = (int)(flatX * TEXTURE_RESOLUTION) % TEXTURE_RESOLUTION;
            float textureY = (int)(flatY * TEXTURE_RESOLUTION) % TEXTURE_RESOLUTION;

            int texture = mapFloors[(int)(flatY) * mapX + (int)(flatX)];
            
            if (texture != 0)
            {
                int red = textureMap[((texture - 1) * TEXTURE_RESOLUTION * TEXTURE_RESOLUTION + (int)textureY * TEXTURE_RESOLUTION + (int)textureX) * 3] * 0.7;
                int green = textureMap[((texture - 1) * TEXTURE_RESOLUTION * TEXTURE_RESOLUTION + (int)textureY * TEXTURE_RESOLUTION + (int)textureX) * 3 + 1] * 0.7;
                int blue = textureMap[((texture - 1) * TEXTURE_RESOLUTION * TEXTURE_RESOLUTION + (int)textureY * TEXTURE_RESOLUTION + (int)textureX) * 3 + 2] * 0.7;

                glColor3ub(red, green, blue);
                glVertex2i(ray, y);
            }

            // nakreslit strop
            texture = mapCeilings[(int)(flatY) * mapX + (int)(flatX)];
            
            if (texture != 0)
            {
                int red = textureMap[((texture - 1) * TEXTURE_RESOLUTION * TEXTURE_RESOLUTION + (int)textureY * TEXTURE_RESOLUTION + (int)textureX) * 3] * 1;
                int green = textureMap[((texture - 1) * TEXTURE_RESOLUTION * TEXTURE_RESOLUTION + (int)textureY * TEXTURE_RESOLUTION + (int)textureX) * 3 + 1] * 1;
                int blue = textureMap[((texture - 1) * TEXTURE_RESOLUTION * TEXTURE_RESOLUTION + (int)textureY * TEXTURE_RESOLUTION + (int)textureX) * 3 + 2] * 1;

                glColor3ub(red, green, blue);
                glVertex2i(ray, screenHeight - y);
            }
        }

        rayAngle = capRad(rayAngle + degToRad(FOV / (screenWidth)));
    }

    glEnd();
}

void drawSky()
{
    int scale = screenHeight / 160;
    
    glPointSize(scale + 1);
    glBegin(GL_POINTS);

    for (int y = 0; y * (scale + 1) < screenHeight / 2; y++)
    {
        for (int x = 0; x < screenWidth / scale; x++)
        {
            int red = skyBox[((int)y * 120 + (int)(x + playerAngle * 240) % 120) * 3];
            int green = skyBox[((int)y * 120 + (int)(x + playerAngle * 240) % 120) * 3 + 1];
            int blue = skyBox[((int)y * 120 + (int)(x + playerAngle * 240) % 120) * 3 + 2];

            glColor3ub(red, green, blue);
            glVertex2i(x * (scale + 1), y * (scale + 1));
        }
    }

    glEnd();
}

int WinMain(void)
{
    GLFWwindow* window;

    /* Initialize the library */
    if (!glfwInit())
        return -1;

    /* Create a windowed mode window and its OpenGL context */
    window = glfwCreateWindow(DEFAULT_WIDTH, DEFAULT_HEIGHT, "Hello World", NULL, NULL);
    if (!window)
    {
        glfwTerminate();
        return -1;
    }

    /* Make the window's context current */
    glfwMakeContextCurrent(window);

    glfwSwapInterval(1);

    glfwSetKeyCallback(window, key_callback);
    glfwSetCursorPosCallback(window, mouse_callback);
    glfwSetInputMode(window, GLFW_CURSOR, GLFW_CURSOR_DISABLED);
    glfwSetWindowSizeCallback(window, window_size_callback);

    glClearColor(0.3, 0.3, 0.3, 0);
    glOrtho(0, DEFAULT_WIDTH, DEFAULT_HEIGHT, 0, -1, 1);
    screenWidth = DEFAULT_WIDTH;
    screenHeight = DEFAULT_HEIGHT;
    
    playerX = 4;
    playerY = 4;
    playerDeltaX = cos(playerAngle);
    playerDeltaY = sin(playerAngle);

    float lastTime = glfwGetTime();

    /* Loop until the user closes the window */
    while (!glfwWindowShouldClose(window))
    {
        // vypocet delta casu
        float deltaTime = glfwGetTime() - lastTime;
        lastTime = glfwGetTime();
        
        // pohyb hrace
        movePlayer(window, deltaTime);

        // renderovani
        glClear(GL_COLOR_BUFFER_BIT);

        drawSky();
        draw3D();

        if (debug)
            drawDebug();

        glfwSwapBuffers(window);

        /* Poll for and process events */
        glfwPollEvents();
    }

    glfwTerminate();
    return 0;
}