#include <GLFW/glfw3.h>
#include <stdbool.h>

#include <stdio.h>
#include <stdlib.h>
#undef max
#undef min
#undef textureMap

#include "util.c"
#include "parser.c"

/* Konstanty */
#define RENDER_DISTANCE 8
#define PLAYER_SIZE 0.3 // velikost hrace v pixelech
#define MOVE_SPEED 3
#define TEXTURE_RESOLUTION 16 // sirka a vyska textur v pixelech
#define DEFAULT_WIDTH 1920 // sirka okna v pixelech
#define DEFAULT_HEIGHT 1080 // vyska okna v pixelch

/* Nastavitelne hodnoty */
float rayResolution = 10; // pocet rendrovanych sloupcu
float fov = 60.0;
float turnSensitivity = 0.002;

/* Globalni promenne */
bool debug = false, menu = false, resetMouse = false;
int mapWidth, mapHeight, playerSpawnX, playerSpawnY, * mapWalls, * mapFloors, * mapCeilings, screenWidth, screenHeight, * textureMap;
char* nextLevel[20];
float playerX, playerY, playerDeltaX, playerDeltaY, playerAngle, deltaTime;

static void respawn()
{
    playerX = playerSpawnX - 0.5;
    playerY = playerSpawnY - 0.5;
    playerAngle = 0;
    playerDeltaX = cos(playerAngle);
    playerDeltaY = sin(playerAngle);
}

static void loadTextureMap()
{
    int count = 0, width = 0, height = 0;
    parseInt("../textures/textureMap.txt", "count", &count);
    parseInt("../textures/textureMap.txt", "width", &width);
    parseInt("../textures/textureMap.txt", "height", &height);

    int textureMapSize = count * width * height * 3;

    textureMap = (int*)malloc(textureMapSize * sizeof(int));
    parseIntArray("../textures/textureMap.txt", "textureMap", textureMap, textureMapSize);
}

static void loadMap(char* name)
{
    /* Precist velikost mapy */
    mapWidth = 0;
    parseInt(name, "width", &mapWidth);
    mapHeight = 0;
    parseInt(name, "height", &mapHeight);
    int mapSize = mapWidth * mapHeight;

    /* Precist mapu */
    mapWalls = (int*)realloc(mapWalls, mapSize * sizeof(int));
    parseIntArray(name, "mapWalls", mapWalls, mapSize);
    mapFloors = (int*)realloc(mapFloors, mapSize * sizeof(int));
    parseIntArray(name, "mapFloors", mapFloors, mapSize);
    mapCeilings = (int*)realloc(mapCeilings, mapSize * sizeof(int));
    parseIntArray(name, "mapCeilings", mapCeilings, mapSize);

    /* Precist spawnpoint hrace */
    playerSpawnX = 0;
    parseInt(name, "spawnX", &playerSpawnX);
    playerSpawnY = 0;
    parseInt(name, "spawnY", &playerSpawnY);

    parseString(name, "nextLevel", nextLevel, 20);

    respawn();
}

void key_callback(GLFWwindow* window, int key, int scancode, int action, int mods)
{
    if (key == GLFW_KEY_F3 && action == GLFW_PRESS)
        debug = !debug;
    if (key == GLFW_KEY_F4 && action == GLFW_PRESS)
        loadTextureMap();
    if (key == GLFW_KEY_ESCAPE && action == GLFW_PRESS)
    {
        menu = !menu;
        if (menu)
            glfwSetInputMode(window, GLFW_CURSOR, GLFW_CURSOR_NORMAL);
        else
            glfwSetInputMode(window, GLFW_CURSOR, GLFW_CURSOR_DISABLED);
        resetMouse = true;
    }
    
    if (key == GLFW_KEY_E && action == GLFW_PRESS)
    {
        switch (mapWalls[(int)(playerY + playerDeltaY * 0.5) * mapWidth + (int)(playerX + playerDeltaX * 0.5)])
        {
        case 2:
            loadMap(nextLevel);
            break;
        }
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

    playerAngle = capRad(playerAngle + xOffset * turnSensitivity);

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

void movePlayer(GLFWwindow* window)
{
    // pokud je v menu, nehybat s hracem
    if (menu)
        return;
    
    float moveX = 0, moveY = 0;

    if (glfwGetKey(window, GLFW_KEY_W) == GLFW_PRESS)
    {
        moveX += playerDeltaX * deltaTime * MOVE_SPEED;
        moveY += playerDeltaY * deltaTime * MOVE_SPEED;
    }

    if (glfwGetKey(window, GLFW_KEY_S) == GLFW_PRESS)
    {
        moveX -= playerDeltaX * deltaTime * MOVE_SPEED;
        moveY -= playerDeltaY * deltaTime * MOVE_SPEED;
    }

    if (glfwGetKey(window, GLFW_KEY_A) == GLFW_PRESS)
    {
        moveX += playerDeltaY * deltaTime * MOVE_SPEED;
        moveY -= playerDeltaX * deltaTime * MOVE_SPEED;
    }

    if (glfwGetKey(window, GLFW_KEY_D) == GLFW_PRESS)
    {
        moveX -= playerDeltaY * deltaTime * MOVE_SPEED;
        moveY += playerDeltaX * deltaTime * MOVE_SPEED;
    }

    // kontrola hitboxu
    float hitboxX1 = playerX + moveX + PLAYER_SIZE * sign(moveX);
    float hitboxX2 = playerX + moveX + PLAYER_SIZE * sign(moveX);
    float hitboxY1 = playerY + PLAYER_SIZE;
    float hitboxY2 = playerY - PLAYER_SIZE;
    
    if (hitboxX1 >= 0 && hitboxX1 < mapWidth && mapWalls[(int)hitboxY1 * mapWidth + (int)hitboxX1] == 0 && mapWalls[(int)hitboxY2 * mapWidth + (int)hitboxX2] == 0)
        playerX += moveX;

    hitboxX1 = playerX + PLAYER_SIZE;
    hitboxX2 = playerX - PLAYER_SIZE;
    hitboxY1 = playerY + moveY + PLAYER_SIZE * sign(moveY);
    hitboxY2 = playerY + moveY + PLAYER_SIZE * sign(moveY);

    if (hitboxY1 >= 0 && hitboxY1 < mapHeight && mapWalls[(int)hitboxY1 * mapWidth + (int)hitboxX1] == 0 && mapWalls[(int)hitboxY2 * mapWidth + (int)hitboxX2] == 0)
        playerY += moveY;
}

void drawDebug()
{
    // nakreslit mapu
    glPointSize(1);
    glBegin(GL_POINTS);

    for (int y = 0; y < TEXTURE_RESOLUTION * mapHeight; y++)
    {
        for (int x = 0; x < TEXTURE_RESOLUTION * mapWidth; x++)
        {
            int texture = mapFloors[(int)(y / TEXTURE_RESOLUTION) * mapWidth + (int)(x / TEXTURE_RESOLUTION)];
            
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

    for (int y = 0; y < TEXTURE_RESOLUTION * mapHeight; y++)
    {
        for (int x = 0; x < TEXTURE_RESOLUTION * mapWidth; x++)
        {
            int texture = mapWalls[(int)(y / TEXTURE_RESOLUTION) * mapWidth + (int)(x / TEXTURE_RESOLUTION)];

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

            glVertex2i(x + TEXTURE_RESOLUTION * (mapWidth + 1), y);
        }
    }

    for (int y = 0; y < TEXTURE_RESOLUTION * mapHeight; y++)
    {
        for (int x = 0; x < TEXTURE_RESOLUTION * mapWidth; x++)
        {
            int texture = mapCeilings[(int)(y / TEXTURE_RESOLUTION) * mapWidth + (int)(x / TEXTURE_RESOLUTION)];

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

            glVertex2i(x + 2 * TEXTURE_RESOLUTION * (mapWidth + 1), y);
        }
    }

    glEnd();

    // nakreslit na mapu hrace
    glColor3f(1, 1, 0);
    glPointSize(4);
    glBegin(GL_POINTS);
    glVertex2i( playerX                   * TEXTURE_RESOLUTION, playerY * TEXTURE_RESOLUTION);
    glVertex2i((playerX +      mapWidth + 1)  * TEXTURE_RESOLUTION, playerY * TEXTURE_RESOLUTION);
    glVertex2i((playerX + 2 * (mapWidth + 1)) * TEXTURE_RESOLUTION, playerY * TEXTURE_RESOLUTION);
    glEnd();

    // ukazat jakym smerem hrac kouka
    glLineWidth(2);
    glBegin(GL_LINES);
    glVertex2i( playerX                   * TEXTURE_RESOLUTION, playerY * TEXTURE_RESOLUTION);
    glVertex2i( playerX                   * TEXTURE_RESOLUTION + playerDeltaX * 10, playerY * TEXTURE_RESOLUTION + playerDeltaY * 10);
    glVertex2i((playerX +      mapWidth + 1)  * TEXTURE_RESOLUTION, playerY * TEXTURE_RESOLUTION);
    glVertex2i((playerX +      mapWidth + 1)  * TEXTURE_RESOLUTION + playerDeltaX * 10, playerY * TEXTURE_RESOLUTION + playerDeltaY * 10);
    glVertex2i((playerX + 2 * (mapWidth + 1)) * TEXTURE_RESOLUTION, playerY * TEXTURE_RESOLUTION);
    glVertex2i((playerX + 2 * (mapWidth + 1)) * TEXTURE_RESOLUTION + playerDeltaX * 10, playerY * TEXTURE_RESOLUTION + playerDeltaY * 10);
    glEnd();
}

void draw3D()
{
    float rayX, rayY, xOffset, yOffset;
    float rayAngle = capRad(playerAngle - degToRad(fov / 2));
    
    glPointSize(rayResolution);
    glBegin(GL_POINTS);

    // pro kazdy paprsek
    for (int ray = 0; ray * rayResolution <= screenWidth; ray++)
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
                int mapPointer = (int)(rayY) * mapWidth + (int)(rayX);

                // pokud je mímo mapu
                if (rayX < 0 || rayX >= mapWidth || rayY < 0 || rayY >= mapHeight)
                {
                    // ulozit trefeneou pozici, vzdalenost
                    horizontalX = rayX;
                    horizontalY = rayY;

                    distanceHorizontal = euclideanDistance(playerX, playerY, horizontalX, horizontalY);
                    horizontalTexture = 1;
                    break;
                }
                
                // pokud trefil
                if (mapWalls[mapPointer] > 0)
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
                int mapPointer = (int)(rayY)*mapWidth + (int)(rayX);

                // pokud je mimo mapu
                if (rayX < 0 || rayX >= mapWidth || rayY < 0 || rayY >= mapHeight)
                {
                    // ulozit trefeneou pozici, vzdalenost
                    verticalX = rayX;
                    verticalY = rayY;

                    distanceVertical = euclideanDistance(playerX, playerY, verticalX, verticalY);
                    verticalTexture = 1;
                    break;
                }
                
                // pokud trefil
                if (mapWalls[mapPointer] > 0)
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
        
        float lineHeight = screenWidth * (RATIO / fov) / distance;

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
            glVertex2i(ray * rayResolution, y + lineOffset);

            textureY += textureYStep;
        }

        // nakreslit podlahu
        for (int y = lineOffset + lineHeight; y < screenHeight; y++)
        {
            float dy = y - (screenHeight / 2.0);

            float flatX = playerX + cos(rayAngle) * screenWidth * RATIO / fov / 2 / dy / cos(changeAngle);
            float flatY = playerY + sin(rayAngle) * screenWidth * RATIO / fov / 2 / dy / cos(changeAngle);

            float textureX = (int)(flatX * TEXTURE_RESOLUTION) % TEXTURE_RESOLUTION;
            float textureY = (int)(flatY * TEXTURE_RESOLUTION) % TEXTURE_RESOLUTION;

            int texture = 0;
            if (flatX >= 0 && flatX < mapWidth && flatY >= 0 && flatY < mapHeight)
                texture = mapFloors[(int)(flatY) * mapWidth + (int)(flatX)];
            
            if (texture != 0)
            {
                int red = textureMap[((texture - 1) * TEXTURE_RESOLUTION * TEXTURE_RESOLUTION + (int)textureY * TEXTURE_RESOLUTION + (int)textureX) * 3] * 0.7;
                int green = textureMap[((texture - 1) * TEXTURE_RESOLUTION * TEXTURE_RESOLUTION + (int)textureY * TEXTURE_RESOLUTION + (int)textureX) * 3 + 1] * 0.7;
                int blue = textureMap[((texture - 1) * TEXTURE_RESOLUTION * TEXTURE_RESOLUTION + (int)textureY * TEXTURE_RESOLUTION + (int)textureX) * 3 + 2] * 0.7;

                glColor3ub(red, green, blue);
                glVertex2i(ray * rayResolution, y);
            }

            // nakreslit strop
            if (flatX >= 0 && flatX < mapWidth && flatY >= 0 && flatY < mapHeight)
                texture = mapCeilings[(int)(flatY) * mapWidth + (int)(flatX)];
            
            if (texture != 0)
            {
                int red = textureMap[((texture - 1) * TEXTURE_RESOLUTION * TEXTURE_RESOLUTION + (int)textureY * TEXTURE_RESOLUTION + (int)textureX) * 3] * 1;
                int green = textureMap[((texture - 1) * TEXTURE_RESOLUTION * TEXTURE_RESOLUTION + (int)textureY * TEXTURE_RESOLUTION + (int)textureX) * 3 + 1] * 1;
                int blue = textureMap[((texture - 1) * TEXTURE_RESOLUTION * TEXTURE_RESOLUTION + (int)textureY * TEXTURE_RESOLUTION + (int)textureX) * 3 + 2] * 1;

                glColor3ub(red, green, blue);
                glVertex2i(ray * rayResolution, screenHeight - y);
            }
        }

        rayAngle = capRad(rayAngle + degToRad(fov / (screenWidth) * rayResolution));
    }

    glEnd();
}

void drawSky()
{
    int width = 0, height = 0;
    parseInt("../textures/skyBox.txt", "width", &width);
    parseInt("../textures/skyBox.txt", "height", &height);

    int skyBoxSize = width * height * 3;

    int* skyBox = (int*)malloc(skyBoxSize * sizeof(int));
    parseIntArray("../textures/skyBox.txt", "skyBox", skyBox, skyBoxSize);
    
    int scale = screenHeight / (height * 2);
    
    glPointSize(scale + 1);
    glBegin(GL_POINTS);

    for (int y = 0; y * (scale + 1) < screenHeight / 2; y++)
    {
        for (int x = 0; x < screenWidth / scale; x++)
        {
            int red = skyBox[((int)y * width + (int)(x + playerAngle * 240) % width) * 3];
            int green = skyBox[((int)y * width + (int)(x + playerAngle * 240) % width) * 3 + 1];
            int blue = skyBox[((int)y * width + (int)(x + playerAngle * 240) % width) * 3 + 2];

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

    float lastTime = glfwGetTime();

    loadTextureMap();

    loadMap("../levels/map_2.txt");

    /* Loop until the user closes the window */
    while (!glfwWindowShouldClose(window))
    {
        // vypocet delta casu
        deltaTime = glfwGetTime() - lastTime;
        lastTime = glfwGetTime();
        
        // pohyb hrace
        movePlayer(window);

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