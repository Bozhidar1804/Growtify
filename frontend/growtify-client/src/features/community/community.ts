import { Component, inject, OnInit, signal } from '@angular/core';
import { MemberService } from '../../core/services/member-service';
import { Observable } from 'rxjs';
import { Member } from '../../types/member';
import { AsyncPipe } from '@angular/common';
import { MemberCard } from "../members/member-card/member-card";
import { PaginatedResult } from '../../types/pagination';
import { Paginator } from "../../shared/paginator/paginator";

@Component({
  selector: 'app-community',
  imports: [AsyncPipe, MemberCard, Paginator],
  templateUrl: './community.html',
  styleUrl: './community.css',
})
export class Community implements OnInit {
  private memberService = inject(MemberService);
  protected paginatedMembers = signal<PaginatedResult<Member> | null>(null);
  pageNumber = 1;
  pageSize = 5;

  ngOnInit(): void {
    this.loadMembers();
  }

  loadMembers() {
    this.memberService.getMembers(this.pageNumber, this.pageSize).subscribe({
      next: result => {
        this.paginatedMembers.set(result);
      }
    })
  }

  onPageChange(event: { pageNumber: number; pageSize: number }) {
    this.pageNumber = event.pageNumber;
    this.pageSize = event.pageSize;
    this.loadMembers();
  }
}
