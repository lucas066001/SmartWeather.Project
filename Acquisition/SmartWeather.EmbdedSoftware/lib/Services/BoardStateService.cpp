#include "BoardStateService.h"

using namespace SmartWeather::Services;

BoardStateService::BoardStateService() : currentState(SmartWeather::Constants::BoardState::PENDING)
{
    pinMode(SmartWeather::Constants::BLUE_DIODE_PIN, OUTPUT);
    pinMode(SmartWeather::Constants::RED_DIODE_PIN, OUTPUT);
    pinMode(SmartWeather::Constants::GREEN_DIODE_PIN, OUTPUT);

    updateLeds();
}

SmartWeather::Constants::BoardState BoardStateService::GetState()
{
    return currentState;
}

void BoardStateService::SetState(SmartWeather::Constants::BoardState newState)
{
    currentState = newState;
    updateLeds();
}

void BoardStateService::BlinkState(SmartWeather::Constants::BoardState newState)
{
    for (int i = 0; i < 5; i++)
    {
        currentState = newState;
        updateLeds();
        delay(200);
        currentState = SmartWeather::Constants::BoardState::DOWN;
        updateLeds();
        delay(200);
    }
}

void BoardStateService::updateLeds()
{
    // Éteindre toutes les LEDs au départ
    digitalWrite(SmartWeather::Constants::BLUE_DIODE_PIN, LOW);
    digitalWrite(SmartWeather::Constants::RED_DIODE_PIN, LOW);
    digitalWrite(SmartWeather::Constants::GREEN_DIODE_PIN, LOW);

    switch (currentState)
    {
    case SmartWeather::Constants::BoardState::PENDING:
        digitalWrite(SmartWeather::Constants::BLUE_DIODE_PIN, HIGH);
        break;
    case SmartWeather::Constants::BoardState::ERROR:
        digitalWrite(SmartWeather::Constants::RED_DIODE_PIN, HIGH);
        break;
    case SmartWeather::Constants::BoardState::OK:
        digitalWrite(SmartWeather::Constants::GREEN_DIODE_PIN, HIGH);
        break;
    case SmartWeather::Constants::BoardState::DOWN:
        break;
    }
}
