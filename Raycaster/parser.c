#include <stdio.h>
#include <string.h>

#define MAX_LENGTH 20 // maximalni delka slova

static void skipUntil(FILE* file, char* valueName)
{
    char s[MAX_LENGTH + 1] = {0};
    
    while (1)
    {
        fscanf_s(file, "%s", s, MAX_LENGTH);
        if (strcmp(s, "#") == 0)
        {
            fscanf_s(file, "%s", s, MAX_LENGTH);
            if (strcmp(s, valueName) == 0)
                return;
        }
    }
}

static void parseInt(char* path, char* valueName, int* output)
{
    FILE* file;
    errno_t error = fopen_s(&file, path, "r");

    if (error)
        return;

    skipUntil(file, valueName);

    fscanf_s(file, "%i", output);

    fclose(file);
}

static void parseString(char* path, char* valueName, char* output, int maxLength)
{
    FILE* file;
    errno_t error = fopen_s(&file, path, "r");

    if (error)
        return;

    skipUntil(file, valueName);

    fscanf_s(file, "%s", output, maxLength);

    fclose(file);
}

static void parseIntArray(char* path, char* valueName, int* output, int size)
{
    FILE* file;
    errno_t error = fopen_s(&file, path, "r");

    if (error)
        return;
    
    skipUntil(file, valueName);

    for (int i = 0; i < size; i++)
        fscanf_s(file, "%i", &output[i]);

    fclose(file);
}