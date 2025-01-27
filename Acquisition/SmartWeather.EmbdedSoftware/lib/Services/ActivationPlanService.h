#ifndef ACTIVATION_PLAN_SERVICE_H
#define ACTIVATION_PLAN_SERVICE_H

namespace SmartWeather::Services
{
    class ActivationPlanService
    {
    public:
        void LaunchNewPlan();
        void StopPlan(int planId);

    private:
        ConnectionService &_connectionService;
        BoardStateService &_boardStateService;
        uint32_t memlast = 0;
    };

}

#endif
