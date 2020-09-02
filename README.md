# Elpida HTTP Rest API

This the web service for handling the results produced by Elpida native application. 
It accepts JSON Results from an authenticated source (via Api Key) and stores it in a document-based (No SQL) 
database. Anonymous users can request the data and query for them. It also handles the binary assets for downloading
and uploading.

## Requirements
* .Net Core 3.1

## Building
Run on root directory:
`dotnet run --project ./src/Elpida.Backend`

## Unit tests
To perform unit tests navigate to 'src' and run:
`dotnet test`

## License

    Elpida Backend
    
    Copyright (C) 2020  Ioannis Panagiotoulos

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Affero General Public License as
    published by the Free Software Foundation, either version 3 of the
    License, or (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Affero General Public License for more details.

    You should have received a copy of the GNU Affero General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.