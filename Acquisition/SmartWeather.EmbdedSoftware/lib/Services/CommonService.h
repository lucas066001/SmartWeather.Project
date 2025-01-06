#ifndef COMMON_SERVICE_H
#define COMMON_SERVICE_H

#include "ConnectionService.h"

namespace SmartWeather::Services {

    class CommonService {
    public:
        CommonService(ConnectionService& connectionService);

        String GenerateGuid();

    private:
        ConnectionService& _connectionService;
    };

} 

#endif
