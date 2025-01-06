#include "CommonService.h"

using namespace SmartWeather::Services;
using namespace SmartWeather::Constants;

CommonService::CommonService(ConnectionService &connectionService, BoardStateService &boardStateService)
    : _connectionService(connectionService),
      _boardStateService(boardStateService)
{
}

String SmartWeather::Services::CommonService::GenerateGuid()
{
  String mac = _connectionService.GetCurrentMacAddress();

  String block1 = String(random(0x10000), HEX); // 4 chiffres hexadécimaux
  String block2 = String(random(0x10000), HEX); // 4 chiffres hexadécimaux
  String block3 = String(random(0x10000), HEX); // 4 chiffres hexadécimaux
  String block4 = String(random(0x10000), HEX); // 4 chiffres hexadécimaux

  String guid = block1 + "-" + block2 + "-" + block3 + "-" + block4 + "-" + mac;
  guid.toUpperCase();

  return guid;
}

void SmartWeather::Services::CommonService::BlockBoardError()
{
  while (true)
  {
    _boardStateService.BlinkState(BoardState::ERROR);
  }
}
