import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanDeactivate, RouterStateSnapshot, UrlTree } from "@angular/router";
import { Observable } from 'rxjs';
import { CheckDeactivate } from './Interface.Guard';

@Injectable({
  providedIn: 'root'
})
export class ConfirmLeaveService implements CanDeactivate<CheckDeactivate>{

  constructor() { }
  canDeactivate(component: CheckDeactivate, currentRoute: ActivatedRouteSnapshot, currentState: RouterStateSnapshot, nextState: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
    return component.checkDeactivate(currentRoute, currentState, nextState);
  }
}
