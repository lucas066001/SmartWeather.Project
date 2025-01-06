#include "HomePage.h"
#include <Arduino.h>

namespace SmartWeather::WebPages {

    // Reusable HTML parts
    const char PAGE_HEADER[] PROGMEM = R"rawliteral(
        <!DOCTYPE html>
        <html lang="en">
        <head>
            <meta charset="UTF-8">
            <meta name="viewport" content="width=device-width, initial-scale=1.0">
            <title>SmartWeather Wi-Fi Config</title>
            <style>
                body { font-family: Arial, sans-serif; margin: 0; padding: 20px; background-color: #f4f4f4; }
                .container { max-width: 600px; margin: 0 auto; background: white; padding: 20px; border-radius: 10px; box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1); }
                h1 { font-size: 24px; text-align: center; }
                p { font-size: 16px; text-align: center; color: #333; }
                .error { color: red; font-weight: bold; }
                .success { color: green; font-weight: bold; }
                form { display: flex; flex-direction: column; }
                input { margin-bottom: 10px; padding: 10px; font-size: 16px; border: 1px solid #ddd; border-radius: 5px; }
                button { padding: 10px; font-size: 16px; background-color: #28a745; color: white; border: none; border-radius: 5px; cursor: pointer; }
                button:hover { background-color: #218838; }
            </style>
        </head>
        <body>
            <div class="container">
                <h1>Configure Wi-Fi</h1>
    )rawliteral";

    const char FORM_HTML[] PROGMEM = R"rawliteral(
                <form action="/SaveParams" method="POST">
                    <label for="ssid">Wi-Fi SSID:</label>
                    <input type="text" id="ssid" name="ssid" required>
                    <label for="password">Wi-Fi Password:</label>
                    <input type="password" id="password" name="password" required>
                    <button type="submit">Save</button>
                </form>
    )rawliteral";

    const char PAGE_FOOTER[] PROGMEM = R"rawliteral(
                <p>These Wi-Fi credentials will be used to connect to your local network.</p>
            </div>
        </body>
        </html>
    )rawliteral";

    // END Reusable HTML parts


    // Format and return Home page
    const char* GetHomePage() {
        static char buffer[2048];
        snprintf_P(buffer, sizeof(buffer), PSTR("%s%s%s"), PAGE_HEADER, FORM_HTML, PAGE_FOOTER);
        return buffer;
    }

    // Format and return Error page
    const char* GetErrorMessagePage(const char* errorMessage) {
        static char buffer[2048];
        snprintf_P(buffer, sizeof(buffer),
            PSTR("%s<p class=\"error\">%s</p>%s%s"),
            PAGE_HEADER, errorMessage, FORM_HTML, PAGE_FOOTER);
        return buffer;
    }

    // Format and return Success page
    const char* GetSuccessPage() {
        static char buffer[2048];
        snprintf_P(buffer, sizeof(buffer),
            PSTR("%s<p class=\"success\">Wi-Fi parameters saved successfully!</p>%s%s"),
            PAGE_HEADER, FORM_HTML, PAGE_FOOTER);
        return buffer;
    }

}
