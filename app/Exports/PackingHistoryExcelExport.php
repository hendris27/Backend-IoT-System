<?php

namespace App\Exports;

use RuntimeException;
use Illuminate\Support\Carbon;
use Symfony\Component\HttpFoundation\BinaryFileResponse;
use ZipArchive;

class PackingHistoryExcelExport
{
    public function download(iterable $users, iterable $processes): BinaryFileResponse
    {
        $temporaryFile = tempnam(sys_get_temp_dir(), 'packing-history-');

        if ($temporaryFile === false) {
            throw new RuntimeException('File sementara untuk export histori tidak dapat dibuat.');
        }

        $archive = new ZipArchive();
        $opened = $archive->open($temporaryFile, ZipArchive::CREATE | ZipArchive::OVERWRITE);

        if ($opened !== true) {
            @unlink($temporaryFile);
            throw new RuntimeException('File Excel histori tidak dapat dibuat.');
        }

        $files = [
            '[Content_Types].xml' => $this->contentTypesXml(),
            '_rels/.rels' => $this->rootRelationshipsXml(),
            'xl/workbook.xml' => $this->workbookXml(),
            'xl/_rels/workbook.xml.rels' => $this->workbookRelationshipsXml(),
            'xl/worksheets/sheet1.xml' => $this->worksheetXml($users),
            'xl/worksheets/sheet2.xml' => $this->detailWorksheetXml($processes),
        ];

        foreach ($files as $name => $contents) {
            if (! $archive->addFromString($name, $contents)) {
                $archive->close();
                @unlink($temporaryFile);
                throw new RuntimeException('Isi file Excel histori tidak dapat ditulis.');
            }
        }

        if (! $archive->close()) {
            @unlink($temporaryFile);
            throw new RuntimeException('File Excel histori tidak dapat disimpan.');
        }

        return response()->download(
            $temporaryFile,
            'rekap-histori-packing-' . now()->format('Ymd_His') . '.xlsx',
            ['Content-Type' => 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'],
        )->deleteFileAfterSend(true);
    }

    private function worksheetXml(iterable $users): string
    {
        $columns = ['A', 'B', 'C', 'D', 'E'];
        $headers = ['No', 'Nama', 'NIK', 'Qty IN (Box)', 'Qty OUT (Box)'];
        $rows = '<row r="1">';

        foreach ($headers as $index => $header) {
            $rows .= $this->stringCell($columns[$index] . '1', $header);
        }

        $rows .= '</row>';
        $rowNumber = 2;
        $number = 1;

        foreach ($users as $user) {
            $values = [
                ['number' => $number++],
                ['text' => $user->name ?: 'Tanpa nama'],
                ['text' => $user->nik ?: '—'],
                ['number' => (int) $user->qty_in],
                ['number' => (int) $user->qty_out],
            ];

            $rows .= '<row r="' . $rowNumber . '">';

            foreach ($values as $index => $value) {
                $reference = $columns[$index] . $rowNumber;
                $rows .= array_key_exists('text', $value)
                    ? $this->stringCell($reference, $value['text'])
                    : $this->numberCell($reference, $value['number']);
            }

            $rows .= '</row>';
            $rowNumber++;
        }

        return '<?xml version="1.0" encoding="UTF-8" standalone="yes"?>'
            . '<worksheet xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main">'
            . '<sheetData>' . $rows . '</sheetData></worksheet>';
    }

    private function detailWorksheetXml(iterable $processes): string
    {
        $columns = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K'];
        $headers = [
            'No', 'Customer', 'Model', 'No HU', 'Lokasi', 'Box', 'Qty',
            'Time IN', 'PIC IN', 'Time OUT', 'PIC OUT',
        ];
        $rows = '<row r="1">';

        foreach ($headers as $index => $header) {
            $rows .= $this->stringCell($columns[$index] . '1', $header);
        }

        $rows .= '</row>';
        $rowNumber = 2;
        $number = 1;

        foreach ($processes as $process) {
            $values = [
                ['number' => $number++],
                ['text' => $process->customer ?: '—'],
                ['text' => $process->model ?: '—'],
                ['text' => $process->no_hu ?: '—'],
                ['text' => $process->location ?: '—'],
                ['text' => $process->box_id ?: '—'],
                ['number' => (int) $process->qty],
                ['text' => $this->formatTime($process->time_in)],
                ['text' => $process->in_pic ?: '—'],
                ['text' => $this->formatTime($process->time_out)],
                ['text' => $process->out_pic ?: '—'],
            ];

            $rows .= '<row r="' . $rowNumber . '">';

            foreach ($values as $index => $value) {
                $reference = $columns[$index] . $rowNumber;
                $rows .= array_key_exists('text', $value)
                    ? $this->stringCell($reference, $value['text'])
                    : $this->numberCell($reference, $value['number']);
            }

            $rows .= '</row>';
            $rowNumber++;
        }

        return '<?xml version="1.0" encoding="UTF-8" standalone="yes"?>'
            . '<worksheet xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main">'
            . '<sheetData>' . $rows . '</sheetData></worksheet>';
    }

    private function formatTime(?string $value): string
    {
        return $value === null
            ? '—'
            : Carbon::parse($value)->timezone('Asia/Bangkok')->format('d M Y, H:i');
    }

    private function stringCell(string $reference, string $value): string
    {
        $value = preg_replace('/[\x00-\x08\x0B\x0C\x0E-\x1F]/u', '', $value) ?? $value;
        $escapedValue = htmlspecialchars($value, ENT_XML1 | ENT_QUOTES, 'UTF-8');

        return '<c r="' . $reference . '" t="inlineStr"><is><t xml:space="preserve">'
            . $escapedValue . '</t></is></c>';
    }

    private function numberCell(string $reference, int $value): string
    {
        return '<c r="' . $reference . '"><v>' . $value . '</v></c>';
    }

    private function contentTypesXml(): string
    {
        return '<?xml version="1.0" encoding="UTF-8" standalone="yes"?>'
            . '<Types xmlns="http://schemas.openxmlformats.org/package/2006/content-types">'
            . '<Default Extension="rels" ContentType="application/vnd.openxmlformats-package.relationships+xml"/>'
            . '<Default Extension="xml" ContentType="application/xml"/>'
            . '<Override PartName="/xl/workbook.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml"/>'
            . '<Override PartName="/xl/worksheets/sheet1.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml"/>'
            . '<Override PartName="/xl/worksheets/sheet2.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml"/>'
            . '</Types>';
    }

    private function rootRelationshipsXml(): string
    {
        return '<?xml version="1.0" encoding="UTF-8" standalone="yes"?>'
            . '<Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">'
            . '<Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument" Target="xl/workbook.xml"/>'
            . '</Relationships>';
    }

    private function workbookXml(): string
    {
        return '<?xml version="1.0" encoding="UTF-8" standalone="yes"?>'
            . '<workbook xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main" '
            . 'xmlns:r="http://schemas.openxmlformats.org/officeDocument/2006/relationships">'
            . '<sheets><sheet name="Rekap Akun" sheetId="1" r:id="rId1"/>'
            . '<sheet name="Detail Transaksi" sheetId="2" r:id="rId2"/></sheets></workbook>';
    }

    private function workbookRelationshipsXml(): string
    {
        return '<?xml version="1.0" encoding="UTF-8" standalone="yes"?>'
            . '<Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">'
            . '<Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet" Target="worksheets/sheet1.xml"/>'
            . '<Relationship Id="rId2" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet" Target="worksheets/sheet2.xml"/>'
            . '</Relationships>';
    }
}
