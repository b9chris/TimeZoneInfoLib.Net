This library helps map DateTimes between TimeZoneInfos. It includes convenience methods for fetching TimeZoneInfo objects, including by state and country, and for switching between timezones.

The core class is DateTimeAndZone, which stores a simple DateTime in UTC format and a TimeZoneInfo object. It offers a Local property for easy conversion to a localized DateTime, and easy switching of TimeZones. Because the underlying data is stored in UTC, the SwitchTimeZone method is literally just changing the TimeZoneInfo object out - the DateTime itself is left alone, avoiding a number of potential conversion issues relating to Daylight Savings transitions.

The state and country lookup is a cheap hash lookup in memory - no databases, no webservices.

The state and country info is incomplete. The US and Canada are mapped, including Hawaii, Alaska, Guam and Puerto Rico, but most of Europe and Asia are not mapped, and Africa is not mapped at all. I welcome contributions to add more countries (and will be adding some myself if the project this was created for requires them).

The state and country lookup is by state/country name, making it easy to map addresses against without an outside webservice or expensive GPS coordinate lookup. However, since it maps exactly one timezone per state, it's inexact for states that have multiple timezones. For example, while CA, US and MA, US are 100% accurate, IN, US is made more difficult by the fact that a small number of counties on the west side of the state are in Central time (and this has changed as laws at the county and state level have passed in the 2000-2010 range). This library maps the entire state as INEST to avoid more complex lookups.


### Legal Crap

Copyright 2012-2013 Chris Moschini, Brass Nine Design

This code is licensed under the LGPL or MIT license, whichever you prefer.

If neither is compatible with your project, contact me at chris@brass9.com, and I'll happily license it to you under whatever it is that meets your licensing restrictions.