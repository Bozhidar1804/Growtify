import { Component, inject } from '@angular/core';
import { MemberService } from '../../core/services/member-service';
import { Observable } from 'rxjs';
import { Member } from '../../types/member';
import { AsyncPipe } from '@angular/common';
import { MemberCard } from "../members/member-card/member-card";

@Component({
  selector: 'app-community',
  imports: [AsyncPipe, MemberCard],
  templateUrl: './community.html',
  styleUrl: './community.css',
})
export class Community {
  private memberService = inject(MemberService);
  protected members$: Observable<Member[]>;

  constructor() {
    this.members$ = this.memberService.getMembers();
  }
}
