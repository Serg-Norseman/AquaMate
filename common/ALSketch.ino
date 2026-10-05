#include <SPI.h>
#include <Ethernet.h>
#include "ALSCommon.h"

// network configuration
byte mac[] = {0xAE, 0xB2, 0x26, 0xE4, 0x4A, 0x5C};
byte ip[] = {192, 168, 0, 100};
byte myDns[] = {192, 168, 0, 1};
byte gateway[] = {192, 168, 0, 1};
byte subnet[] = {255, 255, 255, 0};

EthernetServer server(2000);
EthernetClient client;
String buffer;

void setup() {
  pinMode(led, OUTPUT);

  Serial.begin(115200);
  while (!Serial) {
    ; // wait for serial port to connect. Needed for native USB port only
  }

  // start the Ethernet connection:
  //Ethernet.begin(mac);
  Ethernet.begin(mac, ip);
  //Ethernet.begin(mac, ip, myDns, gateway, subnet);
  server.begin();
  // give the Ethernet shield a second to initialize:
  delay(1000);
  Serial.print("Server address:");
  Serial.println(Ethernet.localIP());
}

void loop() {
  if (Serial.available() > 0) {
    String query = Serial.readString();
    String response = processQuery(query);
    if (response != "") {
      Serial.println(response);
    }
  }

  client = server.available();
  if (client) {
    while (client.available()) {
      char chr = client.read();
      if (chr != '\r') {
        buffer += chr;
      } else {
        //Serial.println(">> " + buffer);
        String response = processQuery(buffer);
        if (response != "") {
          server.println(response);
        }
        buffer = "";
      }
    }
  }
}
