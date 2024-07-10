import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SearchComponent } from '../../components/search/search.component';
import { SearchResultComponent } from '../../components/search-result/search-result.component';



@NgModule({
  declarations: [
    SearchComponent,
    SearchResultComponent
  ],
  imports: [
    CommonModule
  ],
  exports:[
    SearchComponent,
    SearchResultComponent
  ]
})
export class SearchModule { }
