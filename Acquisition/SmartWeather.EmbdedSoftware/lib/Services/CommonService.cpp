#include "CommonService.h"

using namespace SmartWeather::Services;

CommonService::CommonService(ConnectionService &connectionService)
                            : _connectionService(connectionService)
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
