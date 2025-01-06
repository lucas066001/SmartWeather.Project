#ifndef BOARD_STATE_SERVICE_H
#define BOARD_STATE_SERVICE_H

#include <Arduino.h>
#include "DiodeConstants.h"  

namespace SmartWeather::Services {

    class BoardStateService {
    public:
        BoardStateService();

        SmartWeather::Constants::BoardState GetState();
        void SetState(SmartWeather::Constants::BoardState newState);

    private:
        SmartWeather::Constants::BoardState currentState;

        void updateLeds();
    };

} 

#endif
