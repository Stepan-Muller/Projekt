#include <stdio.h>
#include <string.h>

#define MAX_LENGTH 20 // maximalni delka slova

static int skipUntil(FILE* file, char* valueName)
{
    char s[MAX_LENGTH + 1] = { 0 };
    
    while (!feof(file))
    {
        fscanf_s(file, "%s", s, MAX_LENGTH);

        if (strcmp(s, "#") == 0)
        {
            fscanf_s(file, "%s", s, MAX_LENGTH);
            if (strcmp(s, valueName) == 0)
                return 1;
        }
    }

    return 0;
}

static int parseInt(char* path, char* valueName, int* output)
{
    FILE* file;
    errno_t error = fopen_s(&file, path, "r");

    if (error)
        return 0;

    if (skipUntil(file, valueName) == 0) return 0;

    fscanf_s(file, "%i", output);

    fclose(file);

    return 1;
}

static int parseString(char* path, char* valueName, char* output, int maxLength)
{
    FILE* file;
    errno_t error = fopen_s(&file, path, "r");

    if (error)
        return 0;

    if (skipUntil(file, valueName) == 0) return 0;

    fscanf_s(file, "%s", output, maxLength);

    fclose(file);

    return 1;
}

static int parseIntArray(char* path, char* valueName, int* output, int size)
{
    FILE* file;
    errno_t error = fopen_s(&file, path, "r");

    if (error)
        return 0;
    
    if (skipUntil(file, valueName) == 0) return 0;

    for (int i = 0; i < size; i++)
        fscanf_s(file, "%i", &output[i]);

    fclose(file);

    return 1;
}