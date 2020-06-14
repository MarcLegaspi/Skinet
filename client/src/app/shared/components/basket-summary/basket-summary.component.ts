import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { BasketService } from 'src/app/basket/basket.service';
import { Observable, of } from 'rxjs';
import { IBasketItem, IBasket } from '../../models/IBasket';
import { OrderService } from 'src/app/order/order.service';
import { ActivatedRoute } from '@angular/router';
import { IOrderItem } from '../../models/order';

@Component({
  selector: 'app-basket-summary',
  templateUrl: './basket-summary.component.html',
  styleUrls: ['./basket-summary.component.scss']
})
export class BasketSummaryComponent implements OnInit {
  //basket$: Observable<IBasket> = new Observable<IBasket>();
  @Output() increment: EventEmitter<IBasketItem> = new EventEmitter<IBasketItem>();
  @Output() decrement: EventEmitter<IBasketItem> = new EventEmitter<IBasketItem>();
  @Output() remove: EventEmitter<IBasketItem> = new EventEmitter<IBasketItem>();
  @Input() isBasket = true;
  @Input() items: IBasketItem[] | IOrderItem[];
  @Input() isOrder: false;

  constructor(private basketService: BasketService) { }

  ngOnInit(): void {
   //   this.basket$ = this.basketService.basket$;
  }

  incrementItem(item: IBasketItem) {
    this.increment.emit(item);
  }

  decrementItem(item: IBasketItem) {
    this.decrement.emit(item);
  }
  removeBasketItem(item: IBasketItem) {
    this.remove.emit(item);
  }
}
