import { Component, Input } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

interface DataModal {
  label: string;
  isPassword: boolean;
  inputData: string;
}

@Component({
  selector: 'app-change',
  templateUrl: './change.component.html',
  styleUrls: ['./change.component.css'],
})
export class ChangeComponent {
  @Input() dataModal: DataModal;

  constructor(public activeModal: NgbActiveModal) {}
}
