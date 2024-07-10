import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'currencyConversion'
})
export class CurrencyConversionPipe implements PipeTransform {

  private exchangeRate: number = 23000; 

  transform(value: number, ...args: unknown[]): string {
    const vndValue = value * this.exchangeRate;
    return `${vndValue.toLocaleString('vi-VN')} VND`;
  }
  updateExchangeRate(newRate: number): void {
    this.exchangeRate = newRate;
  }
}
