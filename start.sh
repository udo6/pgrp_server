#!/bin/bash

# Define your program and screen session name
PROGRAM="./altv-server"
SCREEN_NAME="gameserver"

# Überprüfung, ob der Bildschirm nicht gefunden wurde, und Erstellung eines neuen Bildschirms
create_screen_if_not_exist() {
    if ! screen -ls | grep -q "$SCREEN_NAME"; then
        echo "Screen session '$SCREEN_NAME' not found. Creating a new one..."
        screen -S "$SCREEN_NAME" -d -m -t "$SCREEN_NAME" bash
        screen -S "$SCREEN_NAME" -p "$SCREEN_NAME" -X stuff "$PROGRAM\n"
    fi
}

while true; do
    # Überprüfung, ob der Bildschirm existiert
    if screen -ls | grep -q "$SCREEN_NAME"; then
        # Überprüfung, ob das Programm läuft
        if ! screen -S "$SCREEN_NAME" -p 0 -X stuff "\004" >/dev/null 2>&1; then
            echo "Program '$PROGRAM' is not running in screen '$SCREEN_NAME'. Restarting..."
            screen -S "$SCREEN_NAME" -X stuff "$PROGRAM\n"
        fi
    else
        create_screen_if_not_exist
    fi
    sleep 10
done