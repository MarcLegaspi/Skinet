import { Component, OnInit } from '@angular/core';
import { BreadcrumbService } from 'xng-breadcrumb';
import { OrderService } from '../order.service';
import { ActivatedRoute } from '@angular/router';
import { IOrder } from 'src/app/shared/models/order';
import { IBasket } from 'src/app/shared/models/IBasket';
import { BasketService } from 'src/app/basket/basket.service';
import { of } from 'rxjs';

@Component({
  selector: 'app-order-detail',
  templateUrl: './order-detail.component.html',
  styleUrls: ['./order-detail.component.scss']
})
export class OrderDetailComponent implements OnInit {
  order: IOrder;

  constructor(private bcService: BreadcrumbService, private orderService: OrderService, 
              private basketService: BasketService, private route: ActivatedRoute) {
    this.bcService.set('@orderDetails', '');
   }

  ngOnInit(): void {
    this.getOrder();
  }

  getOrder() {
    const orderId = +this.route.snapshot.paramMap.get('id');
    this.orderService.getOrder(orderId).subscribe(res => {
      this.bcService.set('@orderDetails', 'Order# ' + res.id.toString() + ' - ' + res.status);
      this.order = res;
    }, error => {
      console.log(error);
    });
  }
}
