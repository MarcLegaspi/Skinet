import { Component, OnInit, Input } from '@angular/core';
import { IBasketTotals } from '../../models/IBasket';
import { Observable } from 'rxjs';
import { BasketService } from 'src/app/basket/basket.service';

@Component({
  selector: 'app-order-totals',
  templateUrl: './order-totals.component.html',
  styleUrls: ['./order-totals.component.scss']
})
export class OrderTotalsComponent implements OnInit {
  @Input() shippingPrice: number;
  @Input() subTotal: number;
  @Input() total: number;

  constructor() { }

  ngOnInit(): void {
  }

}
