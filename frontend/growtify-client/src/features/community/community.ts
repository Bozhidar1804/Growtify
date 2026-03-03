import { Component, inject } from '@angular/core';
import { MemberService } from '../../core/services/member-service';
import { Observable } from 'rxjs';
import { Member } from '../../types/member';
import { AsyncPipe } from '@angular/common';
import { MemberCard } from "../members/member-card/member-card";
import { PaginatedResult } from '../../types/pagination';

@Component({
  selector: 'app-community',
  imports: [AsyncPipe, MemberCard],
  templateUrl: './community.html',
  styleUrl: './community.css',
})
export class Community {
  private memberService = inject(MemberService);
  protected paginatedMembers$: Observable<PaginatedResult<Member>>;

  constructor() {
    this.paginatedMembers$ = this.memberService.getMembers();
  }
}
