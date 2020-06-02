import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-pager',
  templateUrl: './pager.component.html',
  styleUrls: ['./pager.component.scss']
})
export class PagerComponent implements OnInit {
  @Input() pageSize: number;
  @Input() totalCount: number;
  @Output() pageChange = new EventEmitter();
  constructor() { }

  ngOnInit(): void {
  }

  onPagerChanged(event: any)
  {
    this.pageChange.emit(event.page);
  }
}
