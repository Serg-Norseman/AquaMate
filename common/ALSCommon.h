#include <OneWire.h>

const String _version = "v0.1";
const String _sketchName = "ALSketch";

const int led = 13;

char val2hex(uint8_t val)
{
  if ((val & 0x0f) < 10)
    return ('0' + val);
  else
    return ('a' + (val - 10));
}

String arr2str(size_t size, uint8_t *src)
{
  String buffer = "";
  for (size_t i = 0; i < size; i++) {
    uint8_t b = src[i];

    String strByte = "";
    strByte += val2hex(b & 0x0f);
    b >>= 4;
    strByte = val2hex(b & 0x0f) + strByte;

    buffer += strByte;
  }
  return buffer;
}

String getPartFromQuery(String query, int startIndex)
{
  if (startIndex > 0) startIndex++;
  int endIndex = query.indexOf(';', startIndex);
  String part = query.substring(startIndex, endIndex);
  return part;
}

void writeResponse(String response)
{
  delay(50);
  Serial.println(response);
}

void setLED(int pin, int value)
{
  if (value) {
    digitalWrite(pin, HIGH);
  } else {
    digitalWrite(pin, LOW);
  }
}

// OneWire DS18S20, DS18B20, DS1822 Temperature Sensors
String queryTemp(int pin)
{
  OneWire ds(pin);

  uint8_t addr[8];
  byte i = 0;
  while (!ds.search(addr)) {
    // No more addresses
    ds.reset_search();
    delay(250);
    i += 1;
    if (i > 5) return "-100 (No more addresses)";
  }

  String rom = arr2str(8, &addr[0]);

  if (OneWire::crc8(addr, 7) != addr[7]) {
    // CRC is not valid
    return "-101 (CRC is not valid)";
  }

  // the first ROM byte indicates which chip
  byte chipType;
  switch (addr[0]) {
    case 0x10:
      chipType = 1; // Chip = DS18S20
      break;
    case 0x28:
      chipType = 0; // Chip = DS18B20
      break;
    case 0x22:
      chipType = 0; // Chip = DS1822
      break;
    default:
      // Device is not a DS18x20 family device
      return "-102 (Device is not a DS18x20 family device)";
  }

  ds.reset();
  ds.select(addr);
  ds.write(0x44, 1);        // start conversion, with parasite power on at the end

  delay(1000);     // maybe 750ms is enough, maybe not
  // we might do a ds.depower() here, but the reset will take care of it.

  byte present = ds.reset();
  ds.select(addr);
  ds.write(0xBE); // Read Scratchpad

  byte data[12];
  for (i = 0; i < 9; i++) {           // we need 9 bytes
    data[i] = ds.read();
  }

  // Convert the data to actual temperature
  // because the result is a 16 bit signed integer, it should
  // be stored to an "int16_t" type, which is always 16 bits
  // even when compiled on a 32 bit processor.
  int16_t raw = (data[1] << 8) | data[0];
  if (chipType) {
    raw = raw << 3; // 9 bit resolution default
    if (data[7] == 0x10) {
      // "count remain" gives full 12 bit resolution
      raw = (raw & 0xFFF0) + 12 - data[6];
    }
  } else {
    byte cfg = (data[4] & 0x60);
    // at lower res, the low bits are undefined, so let's zero them
    if (cfg == 0x00) raw = raw & ~7;  // 9 bit resolution, 93.75 ms
    else if (cfg == 0x20) raw = raw & ~3; // 10 bit res, 187.5 ms
    else if (cfg == 0x40) raw = raw & ~1; // 11 bit res, 375 ms
    //// default is 12 bit resolution, 750 ms conversion time
  }
  float celsius = (float)raw / 16.0;

  String response = "R:temp;sid:" + rom + ";val:" + celsius + ";";
  return response;
}

int counter = 0;

String processQuery(String query)
{
    counter = counter + 1;

    String response = "";

    query.trim();
    if (query.length() <= 0) return response;
    String command = getPartFromQuery(query, 0);

    if (command == "Q:info") {
      response = "R:ping;sketch:" + _sketchName + ";ver:" + _version + ";counter:" + counter + ";";
    }

    if (command == "Q:setled") {
      String pin = getPartFromQuery(query, command.length());
      String ledValue = getPartFromQuery(query, (command.length() + pin.length() + 1));
      setLED(pin.toInt(), ledValue.toInt());
    }

    if (command == "Q:temp") {
      setLED(13, 1);
      String pin = getPartFromQuery(query, command.length());
      response = queryTemp(pin.toInt());
      setLED(13, 0);
    }

    return response;
}
