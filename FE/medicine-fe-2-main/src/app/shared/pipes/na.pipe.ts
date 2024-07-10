import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'na'
})
export class NaPipe implements PipeTransform {

  transform(value: unknown): unknown {
    if(value !== null && value !== undefined && value !== '' && value !==0){
      return value;
    }
    return 'N/A';
  }
}
