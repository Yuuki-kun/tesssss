import { Injectable, Input } from '@angular/core';
import { ngxCsv } from 'ngx-csv';

@Injectable({
  providedIn: 'root'
})
export class ExportCsvService {

  constructor() { }

  exportCSV(data: any[], fileName: string = "data", selectedFields: string[]){
    const filteredData = data.map(item => {
      const filteredItem: { [key: string]: any } = {};
      selectedFields.forEach(field => {
        filteredItem[field] = item[field];
      });
      return filteredItem;
    });

    const options = {
      fieldSeparator: ',',
      quoteStrings: '"',
      decimalseparator: '.',
      showLabels: true,
      showTitle: true,
      title: fileName,
      useBom: true,
      headers: selectedFields
    };

    new ngxCsv(filteredData, fileName,options);
  }
}
