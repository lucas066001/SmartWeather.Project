#ifndef HOME_PAGE_H
#define HOME_PAGE_H

#include <pgmspace.h>

namespace SmartWeather::WebPages {
    extern const char PAGE_HEADER[] PROGMEM;
    extern const char FORM_HTML[] PROGMEM;
    extern const char PAGE_FOOTER[] PROGMEM;

    const char* GetHomePage();
    const char* GetErrorMessagePage(const char* errorMessage);
    const char* GetSuccessPage();
}

#endif  
