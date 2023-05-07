#include <stdio.h>
#include <string.h>

#define MAX_LENGTH 20 // maximalni delka slova

/* Cte hodnoty ze souboru v urcenem formatu */

/* Preskoci v souboru az do hodnoty s urcenym jmenem */
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

/* Precte cislo z souboru */
static int parseInt(char* path, char* valueName, int* output)
{
    // Nacte soubor
    FILE* file;
    errno_t error = fopen_s(&file, path, "r");
    
    // Pokud soubor neexistuje, vrati error
    if (error)
        return 0;

    // Pokud se nenajde jmeno hdonoty vrati error
    if (skipUntil(file, valueName) == 0) return 0;

    // Precte hodnotu
    fscanf_s(file, "%i", output);

    // Zavre soubor
    fclose(file);

    // Vrati ze funkce probehla spravne
    return 1;
}

/* Precte slovo ze souboru */
static int parseString(char* path, char* valueName, char* output, int maxLength)
{
    // Nacte soubor
    FILE* file;
    errno_t error = fopen_s(&file, path, "r");

    // Pokud soubor neexistuje, vrati error
    if (error)
        return 0;

    // Pokud se nenajde jmeno hdonoty vrati error
    if (skipUntil(file, valueName) == 0) 
        return 0;

    // Precte hodnotu
    fscanf_s(file, "%s", output, maxLength);

    // Zavre soubor
    fclose(file);

    // Vrati ze funkce probehla spravne
    return 1;
}

/* Precte pole cisel ze souboru */
static int parseIntArray(char* path, char* valueName, int* output, int size)
{
    // Nacte soubor
    FILE* file;
    errno_t error = fopen_s(&file, path, "r");

    // Pokud soubor neexistuje, vrati error
    if (error)
        return 0;
    
    // Pokud se nenajde jmeno hdonoty vrati error
    if (skipUntil(file, valueName) == 0) 
        return 0;

    // Precte pole hodnot
    for (int i = 0; i < size; i++)
        fscanf_s(file, "%i", &output[i]);

    // Zavre soubor
    fclose(file);

    // Vrati ze funkce probehla spravne
    return 1;
}