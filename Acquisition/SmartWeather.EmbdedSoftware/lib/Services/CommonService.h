#ifndef COMMON_SERVICE_H
#define COMMON_SERVICE_H

#include "ConnectionService.h"

namespace SmartWeather::Services
{

    class CommonService
    {
    public:
        CommonService(ConnectionService &connectionService, BoardStateService &boardStateService);

        String GenerateGuid();
        void LogHeap();
        void BlockBoardError();

    private:
        ConnectionService &_connectionService;
        BoardStateService &_boardStateService;
        uint32_t memlast = 0;
    };

}

#endif
